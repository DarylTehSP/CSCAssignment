using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Task_5.APIs
{
    public class PhotoController : ApiController
    {

        [HttpPost]
        [Route("api/upload")]
        public async Task<HttpResponseMessage> PostFormData()
        {


            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                List<byte[]> allData = new List<byte[]>();

                foreach (HttpContent content in provider.Contents)
                {
                    allData.Add(await content.ReadAsByteArrayAsync());
                }
                byte[] photo = allData[0];

                Stream photoStm = new MemoryStream(photo);

                string aws_photo_key = Guid.NewGuid().ToString();

                string aws_s3_bucket = "ruriichigyou";

                // PLEASE DELETE BEFORE COMMIT
                string aws_access_id = "";
                string aws_access_key = "";
                string aws_access_token = "";

                SessionAWSCredentials aws_access_cred = new SessionAWSCredentials(aws_access_id, aws_access_key, aws_access_token);

                RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

                AmazonS3Client s3 = new AmazonS3Client(aws_access_cred, bucketRegion);

                TransferUtility fileTransferUtility = new TransferUtility(s3);

                fileTransferUtility.Upload(photoStm, aws_s3_bucket, aws_photo_key);

                string s3_url = String.Format("https://{0}.s3.{1}.amazonaws.com/{2}", aws_s3_bucket, bucketRegion.SystemName, aws_photo_key);

                // PLEASE DELETE BEFORE COMMIT
                string bitly_token = "";

                string requestUrl = "https://api-ssl.bitly.com/v4/shorten";

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

                //Set timeout to 15 seconds
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                request.Headers.Add("Authorization", String.Format("Bearer {0}", bitly_token));
                request.ContentType = "application/json";
                request.Method = "POST";
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {

                    string jsonRequestData = JsonSerializer.Serialize(new { long_url = s3_url });

                    streamWriter.Write(jsonRequestData);
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Console.WriteLine(response.StatusCode);

                JsonDocument jsonDoc = JsonDocument.Parse(response.GetResponseStream());

                JsonElement jsonEle = jsonDoc.RootElement;

                string shorten_link = jsonEle.GetProperty("link").GetString();

                return Request.CreateResponse(HttpStatusCode.OK, new { shortlink = shorten_link });
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

    }
}
