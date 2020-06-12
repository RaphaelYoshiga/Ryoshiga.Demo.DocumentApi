using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RYoshiga.Demo.FunctionApp
{
    public static class GreeterFunction
    {
        [FunctionName("Greeter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var name = await GetName(req);

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        private static async Task<string> GetName(HttpRequest req)
        {
            string name = req.Query["name"];
            if (!string.IsNullOrEmpty(name))
                return name;

            using (var streamReader = new StreamReader(req.Body))
            {
                var requestBody = await streamReader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                return data?.name;
            }
        }
    }
}
