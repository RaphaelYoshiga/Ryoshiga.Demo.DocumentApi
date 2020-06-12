using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RYoshiga.Demo.SmokeTests
{
    public class BaseTest
    {
        protected readonly ConfigurationProvider ConfigurationProvider;

        protected BaseTest()
        {
            ConfigurationProvider = new ConfigurationProvider();
        }

        protected HttpClient GetHttpClient()
        {
            var baseUrl = ConfigurationProvider.GetConfigValue("HostUrl");
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

    }
}
