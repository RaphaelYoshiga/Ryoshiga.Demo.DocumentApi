using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RYoshiga.Demo.Domain;
using RYoshiga.Demo.WebApi.Controllers;
using Shouldly;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RYoshiga.Demo.Specs
{
    [Binding]
    public class SimpleDeliveryApiSteps
    {
        private readonly Mock<IClock> _clockMock;
        private readonly Mock<IRawDeliveryOptionsProvider> _rawDeliveryOptionProvider;
        private ActionResult _actionResult;

        public SimpleDeliveryApiSteps()
        {
            _clockMock = new Mock<IClock>();
            _rawDeliveryOptionProvider = new Mock<IRawDeliveryOptionsProvider>();
        }

        [Given(@"I have those delivery options for country code (.*)")]
        public void GivenIHaveThoseDeliveryOptionsForCountryCode(string countryCode, Table table)
        {
            _rawDeliveryOptionProvider.Setup(p => p.FetchBy(countryCode))
                .ReturnsAsync(table.CreateSet<RawDeliveryOption>());
        }

        [Given(@"the time is (.*)")]
        public void GivenTheTimeIs(string dateTime)
        {
            var now = DateTime.ParseExact(dateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            _clockMock.SetupGet(p => p.UtcNow)
                .Returns(now);
        }

        [When(@"I ask for delivery options for (.*)")]
        public async Task WhenIAskForDeliveryOptionsFor(string countryCode)
        {
            var deliveryOptionsController = GetController();

            _actionResult = await deliveryOptionsController.GetFor(countryCode);
        }

        private DeliveryOptionsController GetController()
        {
            var hostBuilder = new WebApiSpecHostBuilder();
            hostBuilder.AddInstance(_rawDeliveryOptionProvider.Object);
            hostBuilder.AddInstance(_clockMock.Object);
            var host = hostBuilder.Build();

            return (DeliveryOptionsController)host.Services.GetRequiredService(typeof(DeliveryOptionsController));
        }

        [Then(@"I get those delivery options")]
        public void ThenIGetThoseDeliveryOptions(Table table)
        {
            var expectedDeliveryOptions = table.CreateSet<DeliveryOptionResponseExample>().ToList();
            var value = ((OkObjectResult)_actionResult).Value;
            var deliveryOptions = (List<DeliveryOptionResponse>)value;

            deliveryOptions.Count.ShouldBe(expectedDeliveryOptions.Count);

            for (int i = 0; i < deliveryOptions.Count; i++)
            {
                expectedDeliveryOptions[i].AssertAgainst(deliveryOptions[i]);
            }
        }
    }
}
