using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace MemoryBound
{
    public static class Function
    {
        [FunctionName("MemoryBound")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            string input = req.Query["input"];

            if (input == null)
                return new BadRequestObjectResult("Please pass input in the query string");

            int size;
            int.TryParse(input, out size);

            var startTime = DateTime.UtcNow;
            int len = size * 1024 * 1024;
            int[] arr = new int[len];

            // Do some random traversal
            for (int i = 0; i < len; i += 1000)
                arr[i] = i;
            for (int i = 0; i < len; i += 10000)
                arr[i] = i;
            for (int i = 0; i < len; i += 100000)
                arr[i] = i;

            double timeTakenInMs = (DateTime.UtcNow - startTime).TotalMilliseconds;
            long memoryInMB = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64) / (1024 * 1024);

            return (ActionResult)new OkObjectResult($"Input was: {input}, Memory used: {memoryInMB.ToString()}, Time taken: {timeTakenInMs.ToString()}");
        }
    }
}