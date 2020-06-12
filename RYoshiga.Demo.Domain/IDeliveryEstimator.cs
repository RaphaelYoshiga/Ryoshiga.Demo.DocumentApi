using System;

namespace RYoshiga.Demo.Domain
{
    public interface IDeliveryEstimator
    {
        DateTime EstimateDeliveryFor(RawDeliveryOption rawDeliveryOptions);
    }

    public class DeliveryEstimator : IDeliveryEstimator
    {
        private IClock _clock;

        public DeliveryEstimator(IClock clock)
        {
            _clock = clock;
        }

        public DateTime EstimateDeliveryFor(RawDeliveryOption rawDeliveryOptions)
        {
            return _clock.UtcNow
                .AddDays(rawDeliveryOptions.DaysToDispatch)
                .AddDays(rawDeliveryOptions.DaysToDeliver).Date;
        }
    }
}