using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace mayhemFunction
{
    public static class ImageResizer
    {
        
        [FunctionName("ImageResizer")]
        //creando el patron {name}.{ext} azure se encarga de los parametros string name y string ext
        public static async Task  Run(
            [BlobTrigger("resize/{name}", Connection = "AzureWebJobsStorage")]
                Stream myBlob, 

            [Blob("resized/mobile-{name}",FileAccess.Write, Connection ="AzureWebJobsStorage")]
                Stream mobile,

             [Blob("resized/web-{name}",FileAccess.Write, Connection ="AzureWebJobsStorage")]
                Stream web,

             //crear una queue para que se dispare otro proceso
             [Queue("emails", Connection ="AzureWebJobsStorage")]
                IAsyncCollector<string> collector,
            string name
            , ILogger log)
        {
            var processMobile = await ResizeToSmallImageAsync(myBlob);
            await processMobile.CopyToAsync(mobile);
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var processWeb = await ResizeToSmallImageAsync(myBlob);
            await processWeb.CopyToAsync(web);
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var message1 = @"
                {'subject':'Nueva subida de imagen mobile',
                'to':'jorge.arranz@hotmail.com',
                'body':'Se han procesado la imagen mobile'}";

            var message2 = @"
                {'subject':'Nueva subida de imagen web',
                'to':'jorge.arranz@hotmail.com',
                'body':'Se han procesado la imagen web'}";

            await collector.AddAsync(message1);
            await collector.AddAsync(message2);
            

           
            
        }

        private static Task<Stream> ResizeToSmallImageAsync(Stream toResize)
        {
            toResize.Position = 0;
             return Task.FromResult(toResize);
        }
    }
}
