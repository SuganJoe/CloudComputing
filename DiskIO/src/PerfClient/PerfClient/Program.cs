using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PerfClient
{
    enum TestType
    {
        Write,

        WriteAsync,

        Read,

        ReadAsync,

        ReadRandom,

        ReadRandomAsync,

        WriteRead,

        WriteReadRandom
    }

    class Program
    {
        private const string readUrl = "https://diskperf.azurewebsites.net/api/Read";

        private const string readAsyncUrl = "https://diskperf.azurewebsites.net/api/ReadAsync";

        private const string writeUrl = "http://diskperf.azurewebsites.net/api/Write";

        private const string writeAsyncUrl = "https://diskperf.azurewebsites.net/api/WriteAsync";

        private const string deleteUrl = "https://diskperf.azurewebsites.net/api/Delete";

        private const string writeReadUrl = "https://diskperf.azurewebsites.net/api/WriteRead";

        private const int NUMBER_OF_THREADS = 3;

        private const int CALLS_PER_THREAD = 10;

        private const int BLOCK_SIZE = 32 * 1024;

        static void Main(string[] args)
        {
            try
            {
                //for(int i = 0; i < NUMBER_OF_THREADS; ++i)
                //    WriteTest(i).Wait();

                //Console.WriteLine($"Finished writing...");

                Stopwatch sw = Stopwatch.StartNew();
                RunTest(TestType.WriteReadRandom);
                sw.Stop();
                Console.WriteLine($"Test total time for {NUMBER_OF_THREADS * CALLS_PER_THREAD} queries = {sw.Elapsed.TotalMilliseconds:n2}");
                Console.WriteLine("#########################");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        private static void RunTest(TestType testType)
        {
            switch(testType)
            {
                case TestType.Write:
                    Run(WriteTest).Wait();
                    break;

                case TestType.WriteAsync:
                    Run(WriteAsyncTest).Wait();
                    break;

                case TestType.Read:
                    Run(ReadTest).Wait();
                    break;

                case TestType.ReadAsync:
                    Run(ReadAsyncTest).Wait();
                    break;

                case TestType.ReadRandom:
                    Run(ReadRandomTest).Wait();
                    break;

                case TestType.ReadRandomAsync:
                    Run(ReadRandomAsyncTest).Wait();
                    break;

                case TestType.WriteRead:
                    Run(WriteReadTest).Wait();
                    break;

                case TestType.WriteReadRandom:
                    Run(WriteReadRandomTest).Wait();
                    break;

                default:
                    throw new Exception($"Unsupported test type {testType}");
            }
        }

        private static async Task Run(Func<int, Task<List<double>>> action)
        {
            List<Task<List<double>>> tasks = new List<Task<List<double>>>();
            for (int i = 0; i < NUMBER_OF_THREADS; ++i)
                tasks.Add(action(i));

            await Task.WhenAll(tasks.ToArray());

            List<double> latencies = new List<double>();
            foreach (var task in tasks)
                latencies.AddRange(task.Result);

            latencies.Sort();

            int p50 = (int)(latencies.Count * 0.50);
            int p90 = (int)(latencies.Count * 0.90);
            int p99 = (int)(latencies.Count * 0.99);

            Console.WriteLine($"P50 = {latencies[p50]:n2}");
            Console.WriteLine($"P90 = {latencies[p90]:n2}");
            Console.WriteLine($"P99 = {latencies[p99]:n2}");
            Console.WriteLine($"Avg = {latencies.Average():n2}");
            Console.WriteLine($"Sample Size = {latencies.Count}");
        }

        private static async Task RunAsync()
        {
            List<Task<List<double>>> tasks = new List<Task<List<double>>>();
            for (int i = 0; i < 10; ++i)
                tasks.Add(ReadAsyncTest(i));

            await Task.WhenAll(tasks.ToArray());
            List<double> latencies = new List<double>();
            foreach (var task in tasks)
                latencies.AddRange(task.Result);

            latencies.Sort();

            int p50 = (int)(latencies.Count * 0.50);
            int p90 = (int)(latencies.Count * 0.90);
            int p99 = (int)(latencies.Count * 0.99);

            Console.WriteLine($"P50 = {latencies[p50]:n2}");
            Console.WriteLine($"P90 = {latencies[p90]:n2}");
            Console.WriteLine($"P99 = {latencies[p99]:n2}");
            Console.WriteLine($"Avg = {latencies.Average():n2}");
        }

        private static async Task<List<double>> WriteTest(int id)
        {
            List<double> latencies = new List<double>();
            for (int i = 0; i < CALLS_PER_THREAD; ++i)
            {
                string fileName = $"User-{id}-{i}";

                double latency = await RestClient.CallFunction(fileName, writeUrl);
                if(latency != 0.0)
                    latencies.Add(latency);

                //await RestClient.CallFunction(fileName, deleteUrl);
            }

            return latencies;
        }

        private static async Task<List<double>> WriteAsyncTest(int id)
        {
            List<double> latencies = new List<double>();
            for (int i = 0; i < CALLS_PER_THREAD; ++i)
            {
                string fileName = $"User-{id}-{i}";
                double latency = await RestClient.CallFunction(fileName, writeAsyncUrl);
                if(latency != 0.0)
                    latencies.Add(latency);

               await RestClient.CallFunction(fileName, deleteUrl);
            }

            return latencies;
        }

        private static async Task<List<double>> ReadTest(int id)
        {
            List<double> latencies = new List<double>();
            for (int i = 0; i < CALLS_PER_THREAD; ++i)
            {
                string fileName = $"User-{id}-{i}";
                double latency = await RestClient.CallFunction(fileName, readUrl);
                if(latency != 0.0)
                    latencies.Add(latency);
            }

            return latencies;
        }

        private static async Task<List<double>> ReadAsyncTest(int id)
        {
            List<double> latencies = new List<double>();
            for (int i = 0; i < CALLS_PER_THREAD; ++i)
            {
                string fileName = $"User-{id}-{i}";
                double latency = await RestClient.CallFunction(fileName, readAsyncUrl);
                if(latency!= 0.0)
                    latencies.Add(latency);
            }

            return latencies;
        }

        private static async Task<List<double>> ReadRandomTest(int id)
        {
            List<double> latencies = new List<double>();
            for (int i = 0; i < CALLS_PER_THREAD; ++i)
            {
                string fileName = $"User-{id}-{i}";
                double latency = await RestClient.CallFunction(fileName, readUrl, "random");
                if (latency != 0.0)
                    latencies.Add(latency);
            }

            return latencies;
        }

        private static async Task<List<double>> ReadRandomAsyncTest(int id)
        {
            List<double> latencies = new List<double>();
            for (int i = 0; i < CALLS_PER_THREAD; ++i)
            {
                string fileName = $"User-{id}-{i}";
                double latency = await RestClient.CallFunction(fileName, readAsyncUrl, "random");
                if(latency != 0.0)
                    latencies.Add(latency);
            }

            return latencies;
        }

        private static async Task<List<double>> WriteReadTest(int id)
        {
            List<double> latencies = new List<double>();
            for (int i = 0; i < CALLS_PER_THREAD; ++i)
            {
                string fileName = $"User-{id}-{i}";
                double latency = await RestClient.CallFunction2(fileName, writeReadUrl);
                if (latency != 0.0)
                    latencies.Add(latency);
            }

            return latencies;
        }

        private static async Task<List<double>> WriteReadRandomTest(int id)
        {
            List<double> latencies = new List<double>();
            for (int i = 0; i < CALLS_PER_THREAD; ++i)
            {
                string fileName = $"User-{id}-{i}";
                double latency = await RestClient.CallFunction2(fileName, writeReadUrl, "random");
                if (latency != 0.0)
                    latencies.Add(latency);
            }

            return latencies;
        }
    }
}
