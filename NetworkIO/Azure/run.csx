using System.Net;
using System.IO;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req,TraceWriter log)
{   
    log.Info("C# HTTP trigger function processed a request.");
    //var filepath = "https://download.visualstudio.microsoft.com/download/pr/12238090/e8e98d787a47c967cd87330ff959ef9d/VisualStudioforMacInstaller.dmg";
    //var filepath="https://www.eclipse.org/downloads/download.php?file=/technology/epp/downloads/release/kepler/SR1/eclipse-java-kepler-SR1-win32.zip";
    //var filepath="https://media.giphy.com/media/2wZWD8elMd9K4WV3ne/giphy.gif";
    //var filepath="https://www.google.com";
   //string url = req.GetQueryNameValuePairs()
     //   .FirstOrDefault(q => string.Compare(q.Key, "url", true) == 0)
       // .Value;
   
    //if (url == null)
    //{
        // Get request body
      //  dynamic data = await req.Content.ReadAsAsync<object>();
        //url = data?.url;
    //}

    var filepath="https://docs.google.com/document/d/1N2IETL5Bg2ZvyS6B__hfWTAnpbx1hQdhtixojf0njJQ/edit";
    log.Info(filepath);
    WebClient client = new WebClient();
    var watch = System.Diagnostics.Stopwatch.StartNew();
    watch.Restart();
    var output = client.DownloadString(filepath);
    watch.Stop();
    //log.Info("numBytes " + output.Length);
    double elapsedMs = watch.Elapsed.TotalMilliseconds;
    double lengthBytes = output.Length;
    double throughput=(lengthBytes * 1000)/(1024*elapsedMs);
    log.Info("COLD run numBytes in KB: " + lengthBytes/1024);
    log.Info("COLD run Elapsed time in ms: "+ elapsedMs);
    log.Info("COLD run Throughput(KB/sec): " +throughput) ;
    elapsedMs=0;
    lengthBytes=0;
    throughput=0;
    output="";
    watch.Restart();
    for(int i=0;i<3;i++){
        output = client.DownloadString(filepath);
        lengthBytes =lengthBytes+ output.Length;     
    }
    watch.Stop();
    elapsedMs = watch.Elapsed.TotalMilliseconds;
    throughput= (lengthBytes * 1000)/(1024*elapsedMs);

    double avgElapsedMs=elapsedMs/3;
    double avglengthBytes=lengthBytes/3;

    log.Info("Avg run numBytes in KB: " + avglengthBytes/1024);
    log.Info("Avg run Elapsed time in ms: "+ avgElapsedMs);
    log.Info("Avg run Throughput(KB/sec): " +throughput) ;


    return req.CreateResponse(HttpStatusCode.OK, throughput );
}