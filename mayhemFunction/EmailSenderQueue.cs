using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace mayhemFunction
{
    public static class EmailSenderQueue
    {
        //usamos AzureWebJobsStorage pero deberia ser la conexin de azure eso solo para desarrollo
        [FunctionName("EmailSenderQueue")]
        //es para bindear lo que se ha de retornar
        [return: SendGrid(
                ApiKey = "SendGridApiKey",
                From = "jorge.arranz@gmail.com",
                Subject = "{Subject}",
                To = "{To}",
                Text = "{Body}"
            )]
        public static SendGridMessage Run([QueueTrigger("emails", Connection = "AzureWebJobsStorage")]
                Email email,
             
             
            ILogger log)
        {

            log.LogInformation($"C# Queue trigger function processed email");

            return  new SendGridMessage() ;

            
        }
    }
    //public static class EmailSenderQueue
    //{
    //    //usamos AzureWebJobsStorage pero deberia ser la conexin de azure eso solo para desarrollo
    //    [FunctionName("EmailSenderQueue")]
    //    public static void Run([QueueTrigger("emails", Connection = "AzureWebJobsStorage")]
    //            Email email,
    //          [SendGrid(
    //            ApiKey = "SendGridApiKey",
    //            From ="jorge.arranz@gmail.com",
    //            Subject ="{Subject}",
    //            To = "{To}",
    //            Text ="{Body}"
    //        )]
    //           out SendGridMessage sendGridMessage,
    //        ILogger log)
    //    {

    //        sendGridMessage = new SendGridMessage();

    //        log.LogInformation($"C# Queue trigger function processed email");
    //    }
    //}
}
