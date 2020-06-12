using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RYoshiga.Demo.Domain;
using RYoshiga.Demo.WebApi.Controllers;
using Shouldly;
using Xunit;

namespace RYoshiga.Demo.UnitTests
{
    public class DeliveryOptionsControllerShould
    {
        private readonly DeliveryOptionsController _controller;
        private readonly Mock<IRawDeliveryOptionsProvider> _rawDeliveryOptionsProviderMock;
        private readonly Mock<IDeliveryOptionsResponseMapper> _responseMapperMock;

        public DeliveryOptionsControllerShould()
        {
            _rawDeliveryOptionsProviderMock = new Mock<IRawDeliveryOptionsProvider>();
            _responseMapperMock = new Mock<IDeliveryOptionsResponseMapper>();
            _controller = new DeliveryOptionsController(_rawDeliveryOptionsProviderMock.Object, _responseMapperMock.Object);
        }

        [Theory]
        [InlineData("GB")]
        [InlineData("FR")]
        [InlineData("DE")]
        public async Task CallProviderAndReturnMapperResponse(string countryCode)
        {
            var expectedDeliveryOptions = new List<DeliveryOptionResponse> { new DeliveryOptionResponse() };
            var rawDeliveryOptions = new List<RawDeliveryOption> { new RawDeliveryOption() };
            _rawDeliveryOptionsProviderMock.Setup(p => p.FetchBy(countryCode))
                .ReturnsAsync(rawDeliveryOptions);
            _responseMapperMock.Setup(p => p.MapFrom(rawDeliveryOptions))
                .Returns(expectedDeliveryOptions);

            var deliveryOptions = await _controller.GetFor(countryCode);

            ((OkObjectResult)deliveryOptions).Value.ShouldBe(expectedDeliveryOptions);
        }

        [Theory]
        [InlineData("gb", "GB")]
        [InlineData("fr", "FR")]
        public async Task CallDeliveryOptionsFetcherWithUpperedCountryCode(string countryCode, string expectedCountryCode)
        {
            _rawDeliveryOptionsProviderMock.Setup(p => p.FetchBy(expectedCountryCode))
                .ReturnsAsync(new List<RawDeliveryOption>());

            var deliveryOptions = await _controller.GetFor(countryCode);

            _rawDeliveryOptionsProviderMock.Verify(p => p.FetchBy(expectedCountryCode));
        }
    }


}
