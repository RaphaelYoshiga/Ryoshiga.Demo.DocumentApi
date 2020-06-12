using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RYoshiga.Demo.Domain
{
    public class InMemoryRawDeliveryOptionsProvider : IRawDeliveryOptionsProvider
    {
        private static readonly IReadOnlyDictionary<string, List<RawDeliveryOption>> RawDeliveryOptions = new Dictionary<string, List<RawDeliveryOption>>()
        {
            {"GB", new List<RawDeliveryOption>()
                {
                    new RawDeliveryOption
                    {
                        DaysToDispatch = 0,
                        DaysToDeliver = 1,
                        Name = "Next Day Delivery",
                        Price = 10
                    },
                    new RawDeliveryOption
                    {
                        DaysToDispatch = 2,
                        DaysToDeliver = 2,
                        Name = "Standard Delivery",
                        Price = 2.99m
                    }
                }
            },
            {"FR", new List<RawDeliveryOption>()
                {
                    new RawDeliveryOption
                    {
                        DaysToDispatch = 1,
                        DaysToDeliver = 4,
                        Name = "Delivery",
                        Price = 5.5m
                    }
                }
            },
        };

        public Task<IEnumerable<RawDeliveryOption>> FetchBy(string countryCode)
        {
            return Task.FromResult(RawDeliveryOptions[countryCode].AsEnumerable());
        }
    }
}
