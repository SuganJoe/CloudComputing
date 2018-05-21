using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UW.Cloud.Disk.Utils;


namespace UW.Cloud.Disk.Functions
{
    /// <summary>
    /// Serverless function that write file asynchronously
    /// </summary>
    public static class WriteAsync
    {
        [FunctionName("WriteAsync")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string fileName = req.Query["name"];

            Stopwatch sw = Stopwatch.StartNew();
            int bytes = await DiskHelper.DiskWriteAsync(fileName, 1024 * 100);
            sw.Stop();

            double time = bytes == 0 ? 0.0 : sw.Elapsed.TotalMilliseconds;
            string result = $"{time:n2}";
            if (time == 0.0)
            {
                result += $", {Path.Combine(Path.GetTempPath(), fileName)}";
                result += $", {Environment.MachineName}";
            }

            return fileName != null
                ? (ActionResult)new OkObjectResult($"{result}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
