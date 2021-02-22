using BatchTask.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BatchTask.Facade
{
    public static class Util
    {
        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }


        public static void CreateLog(string text)
        {
            string path = Settings.Path_Log;
            string filename = "Log_" + DateTime.Now.ToString("yyyy_MM") + ".txt";

            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            using (StreamWriter writer = new StreamWriter(path + filename, true))
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " " + text);
                writer.Close();
            }
        }


        public static void SendEmail(string message, ParameterMailModel parameterModel)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient();

            var mails = parameterModel.MailTo.Split(';');
            foreach (var item in mails)
            {
                mail.To.Add(item);
            }

            mail.From = new MailAddress(parameterModel.MailFrom);
            mail.Subject = parameterModel.MailSubject;
            mail.IsBodyHtml = true;
            mail.Body = parameterModel.MailMessage + message;
            SmtpServer.Host = parameterModel.MailServer;
            SmtpServer.Port = 25;
            SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Util.CreateLog(ex.Message);
            }
        }
    }
}
