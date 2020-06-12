using Microsoft.Extensions.DependencyInjection;
using RYoshiga.Demo.Domain.Adapters;
using RYoshiga.Demo.Infrastructure;

namespace RYoshiga.Demo.WebApi
{
    public class Ioc
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(new StorageAccountConfiguration()
            {
                ConnectionString = "UseDevelopmentStorage=true"
            });
            services.AddSingleton<IFileSaver, StorageAccountFileManager>();
        }
    }


}