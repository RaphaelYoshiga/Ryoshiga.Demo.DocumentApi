using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using RYoshiga.Demo.WebApi;
using RYoshiga.Demo.WebApi.Controllers;

namespace RYoshiga.Demo.Specs
{
    public class WebApiSpecHostBuilder
    {
        private readonly List<Action<IServiceCollection>> _overrideActions = new List<Action<IServiceCollection>>();

        public IHost Build()
        {
            return GetHostBuilder().Build();
        }

        public void AddInstance<T>(T service) where T : class
        {
            _overrideActions.Add(c => c.AddSingleton<T>(service));
        }

        private IHostBuilder GetHostBuilder()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((c, services) =>
                {
                    var startup = new Startup(c.Configuration);
                    startup.ConfigureServices(services);
                    SetupControllers(services);
                    OverrideServices(services);
                })
                .ConfigureAppConfiguration(SetupConfiguration)
                .ConfigureLogging(b =>
                {
                    b.SetMinimumLevel(LogLevel.Debug);
                    b.AddConsole();
                });

            return host;
        }

        private void OverrideServices(IServiceCollection services)
        {
            services.AddSingleton(Mock.Of<Microsoft.AspNetCore.Hosting.IHostingEnvironment>());
            
            foreach (var overrideAction in _overrideActions)
            {
                overrideAction(services);
            }
        }

        private static void SetupConfiguration(IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();
        }

        private static void SetupControllers(IServiceCollection services)
        {
            services.AddTransient<DocumentApiController>();
        }
    }
}
