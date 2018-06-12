using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambda6
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {

           // int newcontainer = 0;
            Utils ut = new Utils();

            var bodyString = request?.Body;
            //dynamic body = JsonConvert.DeserializeObject(bodyString);
            int count = Int32.Parse(bodyString);


            int newcontainer = 0;
            String uuid = "unset";
            String containerFileName = "/tmp/container-id";

            if (File.Exists(containerFileName))
            {
                using (StreamReader sr = File.OpenText(containerFileName))
                {
                    uuid = sr.ReadLine();
                }
            }
            else
            {
                newcontainer = 1;
                using (StreamWriter sw = new StreamWriter(containerFileName))
                {
                    uuid = Guid.NewGuid().ToString();
                    sw.WriteLine(uuid);
                }
            }


            // Get the CPU stat before running the test
            Utils.CpuTime c1 = ut.GetCpuUtilization();

            // Get the VM CPU stat before running the test
            Utils.VmCpuStat v1 = ut.getVmCpuStat();

            long elapsedTime;

            Stopwatch sw = Stopwatch.StartNew();

            int output = await Task.Run(() => Recursive_Fibonacci(count));

            sw.Stop();
            elapsedTime = sw.ElapsedMilliseconds;


            // Get the CPU stat before running the test 
            Utils.CpuTime c2 = ut.GetCpuUtilization();

            // Get the VM CPU stat after running the test
            Utils.VmCpuStat v2 = ut.getVmCpuStat();

            // Get the CPU and VM CPU stat difference
            Utils.CpuTime cused = ut.getCpuTimeDiff(c1, c2);
            Utils.VmCpuStat vused = ut.getVmCpuStatDiff(v1, v2);

            Response r = new Response(output, elapsedTime);
            /*

            Response r = new Response(uuid, cused.utime, cused.stime, cused.cutime, cused.cstime, vused.cpuusr,
                                  vused.cpunice, vused.cpukrn, vused.cpuidle, vused.cpuiowait, vused.cpuirq,
                                  vused.cpusirq, vused.cpusteal, v2.btime, newcontainer, output);
                                  */

            

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(r.toString())
            };

        }

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
