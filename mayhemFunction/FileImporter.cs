using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace mayhemFunction
{
    public static class FileImporter
    {
        [FunctionName("FileImporter")]
        public static void Run(
            [BlobTrigger("files/{clientId}-{orderType}.{ext}", Connection = "AzureWebJobsStorage")]
                string content,
              [Table("orders", Connection ="AzureWebJobsStorage")]
                ICollector<Order> collector,
            string clientId,
            string orderType,
            ILogger log)
        {
            var orders = content.Split("\n")
                .Select(line =>
                new Order() 
                { 
                    PartitionKey=clientId,
                    RowKey=Guid.NewGuid().ToString(),
                    ClientId=clientId, 
                    OrderType= orderType, 
                    Product=line 
                }).ToList();


            foreach (var order in orders)
            {
                collector.Add(order);
            }
            

            log.LogInformation($"Se ha procesado el archivo");
        }
    }

    public class Order {

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }
        public string ClientId { get; set; }
        public string OrderType { get; set; }
        public string Product { get; set; }
    
    }
}
