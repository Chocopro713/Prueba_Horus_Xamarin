using System;
namespace horus_prueba.Services.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Fusillade;
    using ModernHttpClient;
    using Refit;

    public class ApiService<T> : IApiService<T>
    {
        Func<HttpMessageHandler, T> createClient;
        public ApiService(string apiBaseAddress)
        {
            createClient = messageHandler =>
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress)
                };

                // Add Token
                var token = GlobalSetting.GetInstance().Token;
                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

                // Refit JSON Settings 
                var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());
                return RestService.For<T>(client, refitSettings);
            };
        }

        private T Background
        {
            get
            {
                return new Lazy<T>(() => createClient(
                    new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Background))).Value;
            }
        }

        private T UserInitiated
        {
            get
            {
                return new Lazy<T>(() => createClient(
              new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.UserInitiated))).Value;
            }
        }

        private T Speculative
        {
            get
            {
                return new Lazy<T>(() => createClient(
              new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Speculative))).Value;
            }
        }

        public T GetApi(Priority priority)
        {
            switch (priority)
            {
                case Priority.Background:
                    return Background;
                case Priority.UserInitiated:
                    return UserInitiated;
                case Priority.Speculative:
                    return Speculative;
                default:
                    return UserInitiated;
            }
        }

    }
}
