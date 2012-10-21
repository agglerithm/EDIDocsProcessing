using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace testsmtp
{
    class Program
    {
        static void Main(string[] args)
        {
            var smtp = new SmtpClient();
            smtp.Host = "spammy";

            var msg = new MailMessage("johnreese@austinfoam.com", "johnreese@austinfoam.com");

            msg.Subject = "Testing here";

            msg.Body = "Hello, testing";

//            var cred = new CredentialCache();
//            cred.Add(smtp.Host, smtp.Port,"basic", new NetworkCredential("AFPAutomation",""));

//            smtp.Credentials = cred;       
            smtp.Send(msg);
        }
    }
}
