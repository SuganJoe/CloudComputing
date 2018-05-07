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
        private List<byte[]> AllocateMemory(int input)
        {
            var list = new List<byte[]>();
            int i = 0;
            while (i < 10)
            {
                list.Add(new byte[input * 1024 * 1024]); // Change the size here.
                Thread.Sleep(1000); // Change the wait time here.
                i++;
            }
            return list;
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(int input, ILambdaContext context)
        {
            var l = AllocateMemory(input);
            long memory = GC.GetTotalMemory(false) / (1024 * 1024);
            return "Done " + memory.ToString();
        }
    }
}
