using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace mayhemFunction
{
    //https://www.youtube.com/watch?v=X0-wEGREVls&t=832s
    public static class EmailSenderHttp
    {
        [FunctionName("EmailSenderHttp")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "emails")]
            Email email,
            [SendGrid(
                ApiKey = "SendGridApiKey",
                From ="jorge.arranz@gmail.com",
                Subject ="{Subject}",
                To = "{To}",
                Text ="{Body}"
            )] 
               out SendGridMessage sendGridMessage,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";


            //var content = await textReader.ReadToEndAsync();
            //var email = JsonConvert.DeserializeObject<Email>(content);

            sendGridMessage = new SendGridMessage();

            //Se hace en los parametros de SendGrid en la firma del metodo por eso no tiene que hacerse aqui
            //sendGridMessage = new SendGridMessage();
            //sendGridMessage.SetFrom("jorge.arranz@gmail.com");
            //sendGridMessage.AddTo(email.To);
            //sendGridMessage.AddContent("text/html",email.Body);
            //sendGridMessage.SetSubject(email.Subject);

            return new OkObjectResult($"Email sent to {email.To}");
        }
    }

    public class Email {

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
