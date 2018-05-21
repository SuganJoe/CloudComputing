using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using UW.Cloud.Disk.Utils;

namespace UW.Cloud.Disk.Functions
{
    public static class Delete
    {
        [FunctionName("Delete")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string fileName = req.Query["name"];

            string result = DiskHelper.Delete(fileName);

            return fileName != null
                ? (ActionResult)new OkObjectResult($"0.0, File delete: ({result})")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
