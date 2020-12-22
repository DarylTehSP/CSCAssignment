using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text.Json;

namespace CSC_Assignment1_Task_1
{
    public partial class WeatherServiceForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //https://api.data.gov.sg/v1/environment/24-hour-weather-forecast

            UriBuilder url = new UriBuilder();
            url.Scheme = "https";

            url.Host = "api.data.gov.sg";
            url.Path = "v1/environment/24-hour-weather-forecast";
            //url.Query = "date=2020-12-21";

            ////HTTP request to Singapore NEA 24-hour Weather Forecast web service
            JsonDocument wsResponseJsonDoc = MakeRequest(url.ToString());
            Console.WriteLine(wsResponseJsonDoc);
            if (wsResponseJsonDoc != null)
            {
                //display the JSON response for user
                JsonElement jsonContent = wsResponseJsonDoc.RootElement;
                Response.ContentType = "application/json";
                Response.Write(jsonContent.ToString());
            }
            else
            {
                Response.ContentType = "text/html";
                Response.Write("<h2> Error accessing web service! </h2>");
            }
        }

        public static JsonDocument MakeRequest(string requestUrl)
        // public static XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

                //Set timeout to 15 seconds
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Console.WriteLine(response.StatusCode);

                JsonDocument jsonDoc = JsonDocument.Parse(response.GetResponseStream());
                return jsonDoc;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}