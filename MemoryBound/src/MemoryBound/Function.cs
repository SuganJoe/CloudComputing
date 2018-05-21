using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MemoryBound
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(int input, ILambdaContext context)
        {
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

            return "Done " + memoryInMB.ToString();
        }
    }
}
