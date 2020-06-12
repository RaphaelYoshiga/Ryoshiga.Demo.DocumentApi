using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace RYoshiga.Demo.SmokeTests
{
    public class DeliveryEndpointShould : BaseTest
    {
        [Fact]
        public async Task ReturnDeliveryOptions()
        {
            var httpClient = GetHttpClient();

            var response = await httpClient.GetAsync("deliveryoptions/GB");

            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var deliveryOptionsResponse = JsonConvert.DeserializeObject<List<DeliveryOptionsResponse>>(jsonResponse);
            deliveryOptionsResponse.ShouldNotBeEmpty();
        }
    }
}
