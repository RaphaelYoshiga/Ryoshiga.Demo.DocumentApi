using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using RYoshiga.Demo.Domain;
using Shouldly;
using Xunit;

namespace RYoshiga.Demo.UnitTests
{
    public class DeliveryOptionsResponseMapperShould
    {
        private readonly DeliveryOptionsResponseMapper _mapper;
        private readonly Mock<IDeliveryEstimator> _deliveryEstimatorMock;

        public DeliveryOptionsResponseMapperShould()
        {
            _deliveryEstimatorMock = new Mock<IDeliveryEstimator>();
            _mapper = new DeliveryOptionsResponseMapper(_deliveryEstimatorMock.Object);
        }

        [Theory]
        [InlineData("Super Delivery", 10)]
        [InlineData("Next Day Delivery", 1)]
        public void MapGeneralProperties(string deliveryOptionName, decimal price)
        {
            var rawDeliveryOption = new RawDeliveryOption()
            {
                Name = deliveryOptionName,
                Price = price
            };
            var rawDeliveryOptions = new List<RawDeliveryOption>
            {
                rawDeliveryOption,
                rawDeliveryOption
            };

            var deliveryOptionResponses = _mapper.MapFrom(rawDeliveryOptions).ToList();

            deliveryOptionResponses.Count.ShouldBe(2);
            deliveryOptionResponses[0].Name.ShouldBe(deliveryOptionName);
            deliveryOptionResponses[0].Price.ShouldBe(price);
            deliveryOptionResponses[1].Name.ShouldBe(deliveryOptionName);
            deliveryOptionResponses[1].Price.ShouldBe(price);
        }

        [Fact]
        public void MapDeliveryDateFromEstimator()
        {
            var rawDeliveryOption = new RawDeliveryOption();
            var rawDeliveryOptions = new List<RawDeliveryOption> { rawDeliveryOption };
            DateTime expectedTime = new DateTime(2020, 2, 2);
            _deliveryEstimatorMock.Setup(p => p.EstimateDeliveryFor(rawDeliveryOption))
                .Returns(expectedTime);

            var deliveryOptionResponses = _mapper.MapFrom(rawDeliveryOptions).ToList();

            deliveryOptionResponses.Count.ShouldBe(1);
            deliveryOptionResponses[0].DeliveryDate.ShouldBe(expectedTime);
        }
    }
}
