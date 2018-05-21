using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Diagnostics;
using UW.Cloud.Disk.Utils;

namespace UW.Cloud.Disk.Functions
{
    /// <summary>
    /// Serverless function that writes a file and then reads 10 MB of the file
    /// either sequentially or randomly based on request.
    /// </summary>
    public static class WriteRead
    {
        [FunctionName("WriteRead")]
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
            int bytesWritten = DiskHelper.DiskWrite(fileName, 1024 * 100, delete: true);
            sw.Stop();
            double writeTime = sw.Elapsed.TotalMilliseconds;

            sw.Reset();
            sw.Start();
            int bytesRead = random ? DiskHelper.DiskReadRandom(fileName, blockSize) : DiskHelper.DiskRead(fileName, blockSize);
            sw.Stop();
            double readTime = sw.Elapsed.TotalMilliseconds;

            DiskHelper.Delete(fileName);

            return fileName != null
                ? (ActionResult)new OkObjectResult($"{writeTime:n2},{readTime:n2}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
