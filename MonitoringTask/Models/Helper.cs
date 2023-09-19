using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MonitoringTask.Models
{
    public static class Helper
    {
        public static List<SelectListItem> GetStatusList()
        {
            var statusList = new List<SelectListItem>
            {
                new SelectListItem { Text = "In Progress", Value = "In Progress" },
                new SelectListItem { Text = "Completed", Value = "Completed" },
            };

            return statusList;
        }
        public static List<SelectListItem> GetPriority()
        {
            var statusList = new List<SelectListItem>
            {
                new SelectListItem { Text = "High", Value = "1" },
                new SelectListItem { Text = "Medium", Value = "2" },
                new SelectListItem { Text = "Low", Value = "3" },
            };

            return statusList;
        }

        public static string ReadEmailTemplate(string filePath)
        {
            // Read the file as one string.
            string _fileContent = "";
            System.IO.StreamReader _file = null;
            try
            {
                using (_file = new System.IO.StreamReader(filePath))
                {
                    _fileContent = _file.ReadToEnd();
                    //_file.Close();
                }
            }
            catch (Exception err)
            {
                _fileContent = err.Message;
            }
            finally
            {
                if (_file != null)
                {
                    _file.Close();
                }
            }

            return (_fileContent);
        }
        public static void SendNotif(MonitoringTask.Models.DB.Task task)
        {
            string webUrl = ConfigurationManager.AppSettings["WebURL"];
            string webLocation = ConfigurationManager.AppSettings["WebLocation"];
            string bcc = ConfigurationManager.AppSettings["Bcc"];
            string cc = ConfigurationManager.AppSettings["CC"];
            string emailSubject = ConfigurationManager.AppSettings["EmailSubject"];
            string emailFromName = ConfigurationManager.AppSettings["EmailFromName"];
            string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            string emailLoginId = ConfigurationManager.AppSettings["EmailLoginId"];
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string emailPort = ConfigurationManager.AppSettings["EmailPort"];
            string emailHost = ConfigurationManager.AppSettings["EmailHost"];
            string emailTo = ConfigurationManager.AppSettings["EmailToNotif"];

            string mailContent = ReadEmailTemplate(webLocation + "\\Storage\\EmailTemplates\\Notif.html");

            AttachmentCollection attachments = null;

            mailContent = mailContent.Replace("#Name#", task.Name);
            mailContent = mailContent.Replace("#Desc#", task.Description);
            mailContent = mailContent.Replace("#PIC#", task.PIC);

            SendMail(emailTo, emailFrom, emailFromName, emailLoginId, emailPassword, cc, bcc, emailSubject, mailContent, true, attachments, null, emailHost, int.Parse(emailPort), emailTo);
        }

        public static int SendMail(string mailTo, string mailFrom, string mailFromName, string mailUsername, string mailPassword, string mailCC, string mailBcc, string subject, string body, bool IsHTML, AttachmentCollection attachmentCollection, AlternateView alternateview, string host, int port, string replyToEmail = null)
        {
            int rtnValue = 0;

            if (mailTo.Trim() == "") return -1;

            MailMessage msg = new MailMessage();
            NetworkCredential networkCredential = null;
            msg.From = new MailAddress(mailFrom, mailFromName);
            try
            {
                //msg.To.Add(new MailAddress(mailTo));

                for (int i = 0; i < mailTo.Split(',').Length; i++)
                    msg.To.Add(new MailAddress(mailTo.Split(',')[i]));
            }
            catch { return -1; }

            if (mailCC != null && mailCC != "")
            {
                for (int i = 0; i < mailCC.Split(',').Length; i++)
                    msg.CC.Add(new MailAddress(mailCC.Split(',')[i]));
            }
            // Custom Bcc
            if (mailBcc != null && mailBcc != "")
            {
                for (int i = 0; i < mailBcc.Split(',').Length; i++)
                    msg.Bcc.Add(new MailAddress(mailBcc.Split(',')[i]));
            }

            // Specify the reply-to address
            if (!string.IsNullOrEmpty(replyToEmail))
            {
                msg.ReplyToList.Add(new MailAddress(replyToEmail));
            }

            msg.Subject = subject;
            msg.Body = body;
            if (IsHTML == true)
                msg.IsBodyHtml = true;
            else
                msg.IsBodyHtml = false;

            if (alternateview != null)
            {
                msg.AlternateViews.Add(alternateview);
            }

            if (attachmentCollection != null)
            {
                foreach (Attachment attachment in attachmentCollection)
                {
                    msg.Attachments.Add(attachment);
                }
            }

            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = host;
                client.Port = port;
                if (!String.IsNullOrEmpty(mailUsername) && !String.IsNullOrEmpty(mailPassword))
                {
                    networkCredential = new NetworkCredential(mailUsername, mailPassword);
                    client.UseDefaultCredentials = false;
                    client.Credentials = networkCredential;
                }
                //client.Credentials
                client.Send(msg);
            }
            catch (Exception e)
            {
                rtnValue = -1;
            }

            return rtnValue;
        }

    }

}