using Microsoft.Extensions.DependencyInjection;
using System;

namespace RequestHelper
{
    public static class RequestExtension
    {
        public static IServiceCollection AddRequestHelper(this IServiceCollection services, string Key, string URLBase)
        {
            services.AddScoped<IHttpRequestHelper, HttpRequestHelper>();
            services.AddScoped<IHttpClientMethod, HttpClientMethod>();
            services.AddHttpClient(Key, config => { config.BaseAddress = new Uri(URLBase); });
            return services;
        }

        public static IServiceCollection RegisterAPI(this IServiceCollection services, string Key, string URLBase)
        {
            services.AddHttpClient(Key, config => { config.BaseAddress = new Uri(URLBase); });
            return services;
        }
    }
}
