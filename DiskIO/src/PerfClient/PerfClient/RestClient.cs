using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PerfClient
{
    public class RestClient
    {
        private static HttpClient client = new HttpClient();

        public static async Task<double> CallFunction(string fileName, string baseUrl, string mode = null, int? block = null)
        {
            try
            {
                string url = $"{baseUrl}?name={fileName}";

                if(!string.IsNullOrWhiteSpace(mode))
                {
                    url += $"&mode={mode}";
                }

                if(block != null)
                {
                    url += $"&block={block}";
                }

                var sw = Stopwatch.StartNew();
                string response = await client.GetStringAsync(url);
                sw.Stop();
                Console.WriteLine(response);
                double time = double.Parse(response.Split(',')[0]);
                if (time == 0) return 0.0;
                return sw.Elapsed.TotalMilliseconds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0.0;
            }
        }

        public static async Task<double> CallFunction2(string fileName, string baseUrl, string mode = null, int? block = null)
        {
            try
            {
                string url = $"{baseUrl}?name={fileName}";

                if (!string.IsNullOrWhiteSpace(mode))
                {
                    url += $"&mode={mode}";
                }

                if (block != null)
                {
                    url += $"&block={block}";
                }

                string response = await client.GetStringAsync(url);
                string[] parts = response.Split(',');
                double readTime = double.Parse(parts[1].Trim());

                return readTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0.0;
            }
        }
    }
}
