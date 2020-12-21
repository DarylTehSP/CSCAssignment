using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text.Json;

namespace CSC_Assignment_Task_1
{
    public partial class WeatherServiceForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            // Creating Get Reuquest URL String for NEA Weather Service
            UriBuilder url = new UriBuilder();
            url.Scheme = "https";

            url.Host = "api.gov.data.sg";
            url.Path = "v1/environment/24-hour-weather-forecast";
            url.Query = "date=2020-12-21";

            JsonDocument wsResponseJsonDoc = MakeRequest(url.ToString());

            if(wsResponseJsonDoc!= null)
            {
                JsonElement content = wsResponseJsonDoc.RootElement;
                Response.ContentType = "application/json";
                Response.Write(content.ToString());
            }
            else
            {
                Response.ContentType = "application/json";
                Response.Write("<h2> error accessing web service </h2>");
            }
        }

        public static JsonDocument MakeRequest(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Console.WriteLine(response.StatusCode);
                JsonDocument jsonDoc = JsonDocument.Parse(response.GetResponseStream());
                return jsonDoc;

            }catch (Exception e)    
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }
}