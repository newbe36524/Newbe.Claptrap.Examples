using System;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Ticketing.Repository;
using Newtonsoft.Json;

namespace Newbe.Claptrap.Ticketing.LoadTestClient.Services
{
    public class LoadTestService : ILoadTestService
    {
        private readonly ITrainInfoRepository _trainInfoRepository;
        private readonly ILogger<LoadTestService> _logger;
        private readonly HttpClient _client;
        private readonly Subject<int> _counter;
        private readonly IDisposable _countHandler;

        public LoadTestService(
            ITrainInfoRepository trainInfoRepository,
            ILogger<LoadTestService> logger)
        {
            _trainInfoRepository = trainInfoRepository;
            _logger = logger;
            _client = new HttpClient {BaseAddress = new Uri("http://localhost:10080")};
            _counter = new Subject<int>();
            var totalCount = 0;
            _countHandler = _counter.Subscribe(i =>
            {
                totalCount++;
                _logger.LogInformation("requested {count} count", totalCount * 100);
            });
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("test service started");

            var all = await _trainInfoRepository.GetAllTrainInfoAsync();
            var task = all
                .SelectMany(train =>
                {
                    return Enumerable.Range(0, 10000)
                        .Select(seatId =>
                            SendRequest(new TakeSeatInput
                            {
                                SeatId = string.Empty,
                                TrainId = train.TrainId,
                                FromStationId = train.FromStationId,
                                ToStationId = train.ToStationId,
                            }));
                })
                .ToObservable()
                .Buffer(TimeSpan.FromSeconds(1), 10)
                .Where(x => x.Count > 0)
                .Select(tasks => Observable.FromAsync(async () =>
                {
                    await Task.WhenAll(tasks);
                    _counter.OnNext(1);
                }))
                .Concat()
                .ToTask();
            await task;
        }

        private async Task SendRequest(TakeSeatInput input)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"),
                RequestUri = new Uri("/api/Seat", UriKind.Relative)
            };
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public class TakeSeatInput
        {
            public int TrainId { get; set; }
            public string SeatId { get; set; }
            public int FromStationId { get; set; }
            public int ToStationId { get; set; }
        }
    }
}