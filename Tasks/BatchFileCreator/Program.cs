using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace BatchFileCreator
{
    class Program
    {
        private static readonly string AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        private static readonly string AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
        private static readonly string BUCKET_NAME = Environment.GetEnvironmentVariable("BUCKET_NAME");
        private static readonly RegionEndpoint BUCKET_REGION = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("BUCKET_REGION"));

        private static readonly string APP_PARAM = Environment.GetEnvironmentVariable("APP_PARAM");

        private static IAmazonS3 s3Client;


        static void Main(string[] args)
        {
            //Create a data file
            string filePath = CreateFile();

            //Add file to S3
            s3Client = new AmazonS3Client(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, BUCKET_REGION);
            UploadFileAsync(filePath).Wait();
        }

        //Creates a file to upload to S3
        private static string CreateFile()
        {
            string hostData = "Task_";
            try
            {
                hostData += Dns.GetHostName() + " - ";
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress addr in localIPs)
                {
                    if (addr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        hostData += addr + " ";
                    }
                }
            }
            catch (Exception ex)
            {
            }

            string fileName = hostData + "_" + APP_PARAM + ".txt";

            //Create a file and add data
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.WriteLine(hostData + DateTime.Now.ToLongTimeString());
            }

            return fileName;
        }

        //Uploads file to S3
        private static async Task UploadFileAsync(string filePath)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                await fileTransferUtility.UploadAsync(filePath, BUCKET_NAME);
                Console.WriteLine("Upload 1 completed");

                // Option 2. Specify object key name explicitly.
                //await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                //Console.WriteLine("Upload 2 completed");

                // Option 3. Upload data from a type of System.IO.Stream.
                //using (var fileToUpload =
                //    new FileStream(filePath, FileMode.Open, FileAccess.Read))
                //{
                //    await fileTransferUtility.UploadAsync(fileToUpload,
                //                               bucketName, keyName);
                //}
                //Console.WriteLine("Upload 3 completed");

                // Option 4. Specify advanced settings.
                //var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                //{
                //    BucketName = bucketName,
                //    FilePath = filePath,
                //    StorageClass = S3StorageClass.StandardInfrequentAccess,
                //    PartSize = 6291456, // 6 MB.
                //    Key = keyName,
                //    CannedACL = S3CannedACL.PublicRead
                //};
                //fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                //fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                //await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                //Console.WriteLine("Upload 4 completed");
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
    }
}
