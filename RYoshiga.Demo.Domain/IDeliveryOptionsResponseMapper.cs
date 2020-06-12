using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RYoshiga.Demo.Domain
{
    public interface IDeliveryOptionsResponseMapper
    {
        IEnumerable<DeliveryOptionResponse> MapFrom(IEnumerable<RawDeliveryOption> rawDeliveryOptions);
    }

    public class DeliveryOptionsResponseMapper : IDeliveryOptionsResponseMapper
    {
        private readonly IDeliveryEstimator _deliveryEstimator;

        public DeliveryOptionsResponseMapper(IDeliveryEstimator deliveryEstimator)
        {
            _deliveryEstimator = deliveryEstimator;
        }

        public IEnumerable<DeliveryOptionResponse> MapFrom(IEnumerable<RawDeliveryOption> rawDeliveryOptions)
        {
            return rawDeliveryOptions.Select(Map);
        }

        private DeliveryOptionResponse Map(RawDeliveryOption rawDeliveryOption)
        {
            return new DeliveryOptionResponse()
            {
                Name = rawDeliveryOption.Name,
                Price = rawDeliveryOption.Price,
                DeliveryDate = _deliveryEstimator.EstimateDeliveryFor(rawDeliveryOption)
            };
        }
    }
}