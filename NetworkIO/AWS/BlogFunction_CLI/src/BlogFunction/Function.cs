using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;


using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BlogFunction
{
    public class Function
    {
    public double FunctionHandler(ILambdaContext context)
        {
        	LambdaLogger.Log("C# test");
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            LambdaLogger.Log("C# HTTP trigger function processed a request.");
    //var filepath = "https://download.visualstudio.microsoft.com/download/pr/12238090/e8e98d787a47c967cd87330ff959ef9d/VisualStudioforMacInstaller.dmg";
    //var filepath="http://www.eclipse.org/downloads/download.php?file=/technology/epp/downloads/release/kepler/SR1/eclipse-java-kepler-SR1-linux-gtk.tar.gz";
    //var filepath="https://media.giphy.com/media/2wZWD8elMd9K4WV3ne/giphy.gif";
    //var filepath="https://www.google.com";
    var filepath="https://docs.google.com/document/d/1N2IETL5Bg2ZvyS6B__hfWTAnpbx1hQdhtixojf0njJQ/edit";
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

    

            return throughput;

            
        }
    }
}
