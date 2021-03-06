
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace FiboRESTAsync
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");



            //string requestBody = new StreamReader(req.Body).ReadToEnd();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //int count = data?.number;

            ////Reading the input from HTTP request body
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            int count = Int32.Parse(requestBody);

            long elapsedTime;

            //Calculating elapsed time to find the latency
            Stopwatch sw = Stopwatch.StartNew();


            //Calling Fibo function asynchronously 
            int output = await Task.Run(() => Recursive_Fibonacci(count));
            sw.Stop();

            elapsedTime = sw.ElapsedMilliseconds;

            //Returning responses with fibo output and elapsed time
            Responses r = new Responses(output, elapsedTime);

            
            return (ActionResult)new OkObjectResult(r.toString());
        }

        //Calculating fibonacci series using recurrssion
        public static int Recursive_Fibonacci(int n)
        {
            if (n == 0)
                return 0;
            else if (n == 1)
                return 1;
            else
                return Recursive_Fibonacci(n - 1) + Recursive_Fibonacci(n - 2);
        }
    }
}
