using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using System.Net.Sockets;
using Amazon;
using Amazon.S3.Transfer;
using System.IO;
using System.Net;
using Amazon.S3.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace AccuMarkDeleteMarkerAfterPlot.Controllers
{
    [Route("api/DeleteMarkerAfterPlot")]
    public class DeleteMarkerAfterPlotController : Controller
    {
        private readonly string AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        private readonly string AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
        private static readonly string BUCKET_NAME = "accumarkapi-client-x";
        private static readonly RegionEndpoint BUCKET_REGION = RegionEndpoint.USWest2;
        private static IAmazonS3 s3Client;

         
        // This address must be verified with Amazon SES.
        static readonly string senderAddress = "romeshperera@virtusa.com";


        // The configuration set to use for this email. If you do not want to use a
        // configuration set, comment out the following property and the
        // ConfigurationSetName = configSet argument below. 
        static readonly string configSet = "ConfigSet";

        // The subject line for the email.
        static readonly string subject = "Amazon SES test (AWS SDK for .NET)";

        // The email body for recipients with non-HTML email clients.
        static readonly string textBody = "Amazon SES Test (.NET)\r\n" 
                                        + "This email was sent through Amazon SES "
                                        + "using the AWS SDK for .NET.";
        
        // The HTML body of the email.
        static readonly string htmlBody = @"<html>
<head></head>
<body>
  <h1>Amazon SES Test (AWS SDK for .NET)</h1>
  <p>This email was sent with
     <label>amount </label><span>{{ActivityLog.markerName}}</span><br>
  <label>location </label><span>{{ActivityLog.processStatus}}</span><br>

</body>
</html>";

      
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]PostParms parms)
        {
         Task task = RunAsync(parms.Files, parms.SA,parms.ExpLoc);          
        }

        [HttpPost("{id}")]
        public string Post(int id)
        {
            return "ddd";    
        }

    // PUT api/values/5
    [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
       
        // to check valid marker list
         static async Task<List<ActivityLog>> RunAsync(String[] list,String areaName,String exportlocation)
        {
           // int j = 0;
           // List <String> errorList = new List<string>();
            var activityLog = new List<ActivityLog>(); // list to keep rocords of process status
            
            using (var client = new HttpClient())
            {
                // TODO - Send HTTP requests
                client.BaseAddress = new Uri("http://localhost:60481/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = new TimeSpan(0, 0, 10);
            //check whether the storage area is valid or not by calling storage area service
            //HttpResponseMessage response1 = await client.GetAsync(string.Format("api/storageArea/areaName?{0}",areaName));  
            //if(response1.IsSuccessStatusCode)
            {
              for(int i=0; i< list.Length; i++)
              {           

               //check wether the marker name is valid or not by calling marker service
               //HttpResponseMessage response2 = await client.GetAsync(string.Format("api/marker/markername?{0}",list[i]));

               //if (response2.IsSuccessStatusCode)
               {
                 string filePath = CreateFile(list[i]); //give marker name to plot file

                 //Add file to S3
                 s3Client = new AmazonS3Client(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, BUCKET_REGION);
                 UploadFileAsync(filePath,exportlocation).Wait();

                 //delete marker from s3
                 //HttpResponseMessage response3 = await client.GetAsync(string.Format("api/marker/markername?{0},storagearea?{1}",list[i],areaName));
                 //s3Client = new AmazonS3Client(BUCKET_REGION);
                 //DeleteObjectNonVersionedBucketAsync(filePath).Wait();
                 //if(response3.IsSuccessStatusCode)
                  {
                    activityLog.Add( new ActivityLog {
                         markerName = list[i],
                         processStatus = "marker deleted"
                    });
                    
                  }
                  /*else
                  {
                    activityLog.Add( new ActivityLog {
                         markerName = list[i],
                         processStatus = "process failed"
                    });
                  }*/

                }
                /*else
                {
                    activityLog.Add( new ActivityLog {
                        markerName = list[i],
                        processStatus = "marker notfound"
                    });
                 //errorList[j] = list[i]; //keep marker names which cannot be found to notify the user
                 //j++;
                }*/
               }            
              }
            }
            // return errorList;
            AmozonSES("romesperera@virtusa.com", activityLog);
          return activityLog;
         
        }

         //Creates a file to upload to S3
        private static string CreateFile(string SelectedFile)
        {
            string fileName = SelectedFile + ".plt";

            //Create a file and add data
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.WriteLine("Plot File"); ///
            }

            return fileName;
        }

        //Uploads file to S3
        private static async Task UploadFileAsync(string filePath,string exportlocation)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                await fileTransferUtility.UploadAsync(filePath, string.Format(@"{0}/{1}",BUCKET_NAME,exportlocation));
                Console.WriteLine("Upload 1 completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        //Delete marker from s3 
        private static async Task DeleteObjectNonVersionedBucketAsync(String keyName,String SArea)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = string.Format(@"{0}/{1}/{2}/{3}", BUCKET_NAME, SArea, "Marker", "Made"),
                    Key = keyName
                };

                Console.WriteLine("Deleting an object");
                await s3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
        
        private static void AmozonSES(string receiverAddress, List<ActivityLog> activitylog){
            // Replace USWest2 with the AWS Region you're using for Amazon SES.
            // Acceptable values are EUWest1, USEast1, and USWest2.
            using (var client = new AmazonSimpleEmailServiceClient(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, RegionEndpoint.USWest2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { receiverAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = "<table><tr><td>" + activitylog[0].markerName + "" + activitylog[0].processStatus + "</table></tr></td>"
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBody
                            }
                        }
                    }
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    // ConfigurationSetName = configSet
                };
                try
                {
                    Console.WriteLine("Sending email using Amazon SES...");
                    var response = client.SendEmailAsync(sendRequest);
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);

                }
            }
        }
    }
        
}

    
public class ActivityLog {
    public string markerName { get; set; }
    public string processStatus { get; set; }
}

public class PostParms
{
    public string SA { get; set; }
    public string[] Files { get; set; }
    public string ExpLoc { get; set; }
    public string Email { get; set; }
}