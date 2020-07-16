using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Newbe.Claptrap.Ticketing.BlazorDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            //������ز���� webapi �����ַ���ԣ�������Լ���url��ַ
            string baseUrl = "https://localhost:36524/";
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });
            builder.Logging.SetMinimumLevel(LogLevel.Debug);//������־
            builder.Services.AddHttpClient("train", (s, h) =>
            {
                h.BaseAddress = new Uri(baseUrl);
            });
            await builder.Build().RunAsync();
        }
    }
}
