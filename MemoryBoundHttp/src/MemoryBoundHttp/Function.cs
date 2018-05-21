using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MemoryBoundHttp
{
    public class Function
    {
        
        /// <summary>
        /// Memory bound
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var bodyString = request?.Body;
            int input = Int32.Parse(bodyString);

            var startTime = DateTime.UtcNow;
            int len = input * 1024 * 1024;
            int[] arr = new int[len];
            for (int i = 0; i < len; i += 1000)
                arr[i] = i;
            for (int i = 0; i < len; i += 10000)
                arr[i] = i;
            for (int i = 0; i < len; i += 100000)
                arr[i] = i;

            Console.WriteLine((DateTime.UtcNow - startTime).TotalMilliseconds);
            long memoryInMB = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64) / (1024*1024);
            Console.WriteLine(memoryInMB);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(memoryInMB)
            };
        }
    }
}
