using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Diagnostics;
using System.IO;
using UW.Cloud.Disk.Utils;


namespace UW.Cloud.Disk.Functions
{
    /// <summary>
    /// Serverless function that reads file sequentially
    /// </summary>
    public static class Read
    {
        [FunctionName("Read")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string fileName = req.Query["name"];
            string blockSizeStr = req.Query["block"];
            string mode = req.Query["mode"];

            int blockSize;
            if (!int.TryParse(blockSizeStr, out blockSize))
            {
                blockSize = 1024 * 10;
            }

            bool random = false;
            random = string.IsNullOrWhiteSpace(mode) ? false : (mode == "random");

            Stopwatch sw = Stopwatch.StartNew();
            int fileSize = random ? DiskHelper.DiskReadRandom(fileName, blockSize) : DiskHelper.DiskRead(fileName, blockSize);
            sw.Stop();

            double time = fileSize == 0 ? 0.0 : sw.Elapsed.TotalMilliseconds;
            string result = $"{time:n2}";
            if (time == 0.0)
            {
                result += $", {Path.Combine(Path.GetTempPath(), fileName)}";
                result += $", {Environment.MachineName}";
            }

            return (ActionResult)new OkObjectResult(result);
        }
    }
}
