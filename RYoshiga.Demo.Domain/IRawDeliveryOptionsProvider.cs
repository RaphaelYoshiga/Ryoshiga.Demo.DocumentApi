using System.Collections.Generic;
using System.Threading.Tasks;

namespace RYoshiga.Demo.Domain
{
    public interface IRawDeliveryOptionsProvider
    {
        Task<IEnumerable<RawDeliveryOption>> FetchBy(string countryCode);
    }
}