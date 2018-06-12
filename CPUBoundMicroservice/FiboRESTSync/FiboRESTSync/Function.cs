using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Diagnostics;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace FiboRESTSync
{
    public class Function
    {


        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var bodyString = request?.Body;
            //dynamic body = JsonConvert.DeserializeObject(bodyString);
            int count = Int32.Parse(bodyString);
            long elapsedTime;

            //Calculating elapsed time to find the latency
            Stopwatch sw = Stopwatch.StartNew();

            int output = Recursive_Fibonacci(count);

            //sw.Stop();
            //long elapsedTime = sw.ElapsedTicks;

            sw.Stop();
            elapsedTime = sw.ElapsedMilliseconds;

            //Returning responses with fibo output and elapsed time
            Responses r = new Responses(output, elapsedTime);
            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(r.toString())
            };


        }

        //Calculating fibonacci series using recurrssion
        public int Recursive_Fibonacci(int n)
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

