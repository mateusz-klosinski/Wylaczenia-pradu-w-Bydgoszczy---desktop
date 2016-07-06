using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EneaWPF
{
    class HandleEmail
    {
        private string server;
        private int port;

        public HandleEmail(string server, int port)
        {
            this.server = server;
            this.port = port;
        }


        public bool sendMail(string subject, string message, string to)
        {
            using (var client = new SmtpClient(server, port))
            {
                client.Credentials = new System.Net.NetworkCredential("przerwywdostawach@gmail.com", "eneaoperator123");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;

                using (var mail = new MailMessage("przerwywdostawachpradu@gmail.com", to, subject, message))
                {
                    try
                    {
                        client.Send(mail);
                        return true;
                    }
                    catch (SmtpException)
                    {
                        MessageBox.Show("Nie udało się wysłać wiadomości e-mail.", "Nie wysłano wiadomości!");
                        return false;
                    }
                }
            }
        }
    }
}
