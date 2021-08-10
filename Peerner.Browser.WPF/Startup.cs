using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class Startup
    {
        private readonly IServiceProvider _serviceProvider;

        public Startup()
        {
            _serviceProvider = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .Build()
                .Services;
        }

        //public MainViewModel Main =>
        //    _serviceProvider.GetService<MainViewModel>();

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IApiClient, ApiClient>(client =>
                {
                    client.BaseAddress = new Uri("");
                });
                //.AddPolicyHandler(GetRetryPolicy());
            
            
            //services.AddSingleton<MainViewModel>();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
