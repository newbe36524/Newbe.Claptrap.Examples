using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Newbe.Claptrap.Auth.TestConsole
{
    class Program
    {
        private static HttpClient _client = new HttpClient();

        static async Task Main(string[] args)
        {
            const string baseUri = "http://localhost:10080/";
            _client = new HttpClient {BaseAddress = new Uri(baseUri)};

            string s;
            do
            {
                Console.WriteLine("please input something:");
                s = Console.ReadLine();
                if (int.TryParse(s, out var count))
                {
                    var pageSize = 100;
                    var pageCount = count * 1.0 / pageSize;
                    for (int i = 0; i < pageCount; i++)
                    {
                        var sw = Stopwatch.StartNew();
                        var start = i * pageSize;
                        var end = Math.Min(count, (i + 1) * pageSize);
                        var tasks = Enumerable.Range(start, end - start)
                            .Select(id => LoginAsync(id.ToString()));
                        await Task.WhenAll(tasks);
                        Console.WriteLine(sw.ElapsedMilliseconds);
                    }
                }
            } while (s != "exit");
        }

        private static async Task<UserToken> LoginAsync(string id)
        {
            var resp = await _client.GetAsync($"/api/user/{id}?username=User{id}&password={id}pwd");
            if (resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                var userToken = JsonConvert.DeserializeObject<UserToken>(body);
                return userToken;
            }
            else
            {
                var body = await resp.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return default;
            }
        }

        private static async Task Validate(string validateUrl)
        {
            var validateResp = await _client.GetAsync(validateUrl);
            validateResp.EnsureSuccessStatusCode();
            var validateJson = await validateResp.Content.ReadAsStringAsync();
            if (validateJson != "true")
            {
                throw new Exception("validate error");
            }
        }
    }

    public class UserToken
    {
        public string Token { get; set; }
        public string Validate { get; set; }
    }
}