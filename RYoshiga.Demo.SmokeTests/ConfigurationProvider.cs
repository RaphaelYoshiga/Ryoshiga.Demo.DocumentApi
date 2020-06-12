using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace RYoshiga.Demo.SmokeTests
{
    public class ConfigurationProvider
    {
        private readonly IConfigurationRoot _configurationBuilder;
        private readonly string _environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public ConfigurationProvider()
        {
            string hostingEnvironment = _environmentVariable ?? "Development";
            Console.WriteLine($"Running with hosting: {hostingEnvironment}");

            _configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("smokeTestSettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"smokeTestSettings.{hostingEnvironment.ToLowerInvariant()}.json", optional: true,
                    reloadOnChange: true)
                .Build();
        }

        public string GetConfigValue(string key)
        {
            var configurationSection = _configurationBuilder.GetSection(key);

            return configurationSection?.Value;
        }
    }
}
