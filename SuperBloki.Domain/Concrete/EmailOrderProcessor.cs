using System.Net;
using System.Net.Mail;
using System.Text;
using SuperBloki.Domain.Abstract;
using SuperBloki.Domain.Entities;

namespace SuperBloki.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "superbloki@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"C:\Users\Admin\source\repos\SuperBloki\super_bloki_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Nowe zamówienie zostało przetworzone")
                    .AppendLine("---")
                    .AppendLine("Towary:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Constructor.Priсe * line.Quantity;
                    body.AppendFormat("{0} x {1} (razem: {2:c}",
                        line.Quantity, line.Constructor.Name, subtotal);
                }

                body.AppendFormat("Całkowity koszt: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Dostawa:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine("---")
                    .AppendFormat("Papier ozdobny: {0}",
                        shippingInfo.GiftWrap ? "Tak" : "Nie");

                MailMessage mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,	// Od kogo
                                       emailSettings.MailToAddress,		// Do kogo
                                       "Wysłano nowe zamówienie!",		// Temat
                                       body.ToString()); 				// Treść listu

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}
