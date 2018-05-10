using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TittleAdmin.Service.Implementations
{
    public class MailData
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class MailService
    {
        public static bool SendEmail(MailData _objModelMail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(Convert.ToString(ConfigurationManager.AppSettings["FromEmailAddress"]));
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                new Thread(() =>
                {
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (Exception ex)
                    {
                    }
                }).Start();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
