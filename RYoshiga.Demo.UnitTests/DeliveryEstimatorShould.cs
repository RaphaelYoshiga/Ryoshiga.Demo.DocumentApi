using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using RYoshiga.Demo.Domain;
using Shouldly;
using Xunit;

namespace RYoshiga.Demo.UnitTests
{
    public class DeliveryEstimatorShould
    {
        private readonly Mock<IClock> _clockMock;
        private DeliveryEstimator _estimator;
        private readonly DateTime _firstJan2020 = new DateTime(2020, 1, 1);

        public DeliveryEstimatorShould()
        {
            _clockMock = new Mock<IClock>();
            _estimator = new DeliveryEstimator(_clockMock.Object);

            _clockMock.SetupGet(p => p.UtcNow)
                .Returns(_firstJan2020);
        }

        [Fact]
        public void EstimateNextDayDelivery()
        {
            var rawDeliveryOptions = new RawDeliveryOption
            {
                DaysToDispatch = 0,
                DaysToDeliver = 1
            };

            var deliveryDate = _estimator.EstimateDeliveryFor(rawDeliveryOptions);

            deliveryDate.ShouldBe(new DateTime(2020, 1, 2));
        }

        [Fact]
        public void EstimateStandardDelivery()
        {
            var rawDeliveryOptions = new RawDeliveryOption
            {
                DaysToDispatch = 1,
                DaysToDeliver = 2
            };

            var deliveryDate = _estimator.EstimateDeliveryFor(rawDeliveryOptions);

            deliveryDate.ShouldBe(new DateTime(2020, 1, 4));
        }

        [Fact]
        public void EstimateLowPriorityDelivery()
        {
            var rawDeliveryOptions = new RawDeliveryOption
            {
                DaysToDispatch = 5,
                DaysToDeliver = 5
            };

            var deliveryDate = _estimator.EstimateDeliveryFor(rawDeliveryOptions);

            deliveryDate.ShouldBe(new DateTime(2020, 1, 11));
        }
    }
}
