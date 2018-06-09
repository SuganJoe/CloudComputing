using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;





// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BlogFunction
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
            WebClient client = new WebClient();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var output = client.DownloadString(filepath);
            watch.Stop();
            LambdaLogger.Log("numBytes " + output.Length);
            double elapsedMs = watch.Elapsed.TotalMilliseconds;
            double lengthBytes = output.Length;
            double throughput=(lengthBytes * 1000)/(1024*elapsedMs);
            LambdaLogger.Log("COLD run numBytes in KB: " + lengthBytes/1024);
            LambdaLogger.Log("COLD run Elapsed time in ms: "+ elapsedMs);
            LambdaLogger.Log("COLD run Throughput(KB/sec): " +throughput) ;
            elapsedMs=0;
            lengthBytes=0;
            throughput=0;
            output = "";
    
            watch.Restart();
            for(int i=0;i<3;i++){
                output = client.DownloadString(filepath);
                lengthBytes =lengthBytes+ output.Length;    
            }
            watch.Stop();
            elapsedMs = watch.Elapsed.TotalMilliseconds;
            throughput= (lengthBytes * 1000)/(1024*elapsedMs);

            double avgElapsedMs=elapsedMs/1;
            double avglengthBytes=lengthBytes/1;

            LambdaLogger.Log("Avg run numBytes in KB: " + avglengthBytes/1024);
            LambdaLogger.Log("Avg run Elapsed time in ms: "+ avgElapsedMs);
            LambdaLogger.Log("Avg run Throughput(KB/sec): " +throughput) ;


            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(throughput)
            };
        }
    }
}


