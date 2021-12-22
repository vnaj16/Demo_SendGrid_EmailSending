using System;
using System.IO;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Demo_SendGrid_EmailSending
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

            string FilePath = Environment.CurrentDirectory + @"\index.html";
            string PDFPath = Environment.CurrentDirectory + @"\testfile.pdf";
            string htmlFile = File.ReadAllText(FilePath);
            string pdfFileBase64Encoded = Convert.ToBase64String(File.ReadAllBytes(PDFPath));

            var employee = new Employee()
            {
                Company = "VNAJ INC.",
                Name = "Arthur"
            };

            htmlFile = htmlFile.Replace("{{Nombre}}", employee.Name).Replace("{{Company}}", employee.Company);


            #region SendGrid
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("example@domain.com", "AJ Developer");
            var subject = "Sending with Twilio SendGrid is Fun";
            var to = new EmailAddress("example@domain.com", "AJ Student");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = htmlFile;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            msg.AddAttachment("testFile.pdf", pdfFileBase64Encoded);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            Console.WriteLine(response.StatusCode + " - " + response.Body + " (" +response.Headers + ")");
            #endregion

            Console.ReadLine();
        }

        class Employee
        {
            public string Name { get; set; }
            public string Company { get; set; }
        }
    }
}
