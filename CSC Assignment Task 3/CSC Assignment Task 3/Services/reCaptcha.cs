using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace CSC_Assignment_Task_3.Services
{
    public class reCaptcha
    {
        public bool VerifyCaptcha(string token)
        {
            string captchaSecret = "";

            WebRequest request = WebRequest.Create("https://www.google.com/recaptcha/api/siteverify");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string postData = String.Format("secret={0}&response={1}", captchaSecret, token);
            byte[] postDataBtyes = Encoding.UTF8.GetBytes(postData);

            request.ContentLength = postDataBtyes.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(postDataBtyes, 0, postDataBtyes.Length);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            JsonDocument captchaResponse = JsonDocument.Parse(response.GetResponseStream());
            JsonElement captchaResponseJson = captchaResponse.RootElement;

            double recaptchaScore = captchaResponseJson.GetProperty("score").GetDouble();
            if (recaptchaScore > 0.5)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}