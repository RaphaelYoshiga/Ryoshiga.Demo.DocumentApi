using Microsoft.Extensions.DependencyInjection;
using RYoshiga.Demo.Domain;

namespace RYoshiga.Demo.WebApi
{
    public class Ioc
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IDeliveryEstimator, DeliveryEstimator>();
            services.AddSingleton<IRawDeliveryOptionsProvider, InMemoryRawDeliveryOptionsProvider>();
            services.AddSingleton<IDeliveryOptionsResponseMapper, DeliveryOptionsResponseMapper>();
            services.AddSingleton<IClock, Clock>();
        }
    }


}