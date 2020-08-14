using System;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
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

        public LoadTestService(
            ITrainInfoRepository trainInfoRepository,
            ILogger<LoadTestService> logger)
        {
            _trainInfoRepository = trainInfoRepository;
            _logger = logger;
            _client = new HttpClient {BaseAddress = new Uri("http://localhost:36525")};
        }

        public async Task RunAsync()
        {
            var all = await _trainInfoRepository.GetAllTrainInfoAsync();
            var task = Enumerable.Range(0, 10000)
                .SelectMany(seatId => all.Select(train => SendRequest(new TakeSeatInput
                {
                    SeatId = "-1",
                    TrainId = train.TrainId,
                    FromStationId = train.FromStationId,
                    ToStationId = train.ToStationId,
                })))
                .Select(x => Observable.FromAsync(() => x))
                .ToObservable()
                .Merge(100)
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