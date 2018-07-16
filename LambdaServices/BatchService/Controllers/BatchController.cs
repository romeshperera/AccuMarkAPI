using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Amazon.ECS;
using Amazon.ECS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AccuBatch.Controllers
{
    [Produces("application/json")]
    [Route("Batch")]
    public class BatchController : Controller
    {
        private readonly string AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_IDX");
        private readonly string AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEYX");
        private readonly string TASK_DEFINITION = Environment.GetEnvironmentVariable("TASK_DEFINITION");
        private readonly string CLUSTER = Environment.GetEnvironmentVariable("CLUSTER");
        private readonly string CONTAINER_NAME = Environment.GetEnvironmentVariable("CONTAINER_NAME");
        private readonly string LAUNCH_TYPE = LaunchType.FindValue(Environment.GetEnvironmentVariable("LAUNCH_TYPE"));
        private readonly int COUNT = int.Parse(Environment.GetEnvironmentVariable("COUNT"));

        // GET: Batch
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string core = String.Empty;
            string host = String.Empty;

            try
            {
                core = SampleWapper.Wapper.Add(1, 2).ToString();
            }
            catch (Exception ex)
            {
                core = "FAILED - " + ex.Message;
            }

            try
            {
                core += Add(1, 2).ToString();
            }
            catch (Exception ex)
            {
                core += "XXXFAILED - " + ex.Message;
            }

            try
            {
                host = Dns.GetHostName() + " - ";
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress addr in localIPs)
                {
                    if (addr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        host = addr + " ";
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return new string[] { "CppWapper: " + core, "HostIP: " + host };
        }

        // GET: Batch/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            
            return "Batch Task - " + RunNewTask(id.ToString());
        }
        
        // POST: Batch
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: Batch/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private string RunNewTask(string input)
        {
            string result = "Success - ";
            try
            {
                //Create task request
                RunTaskRequest taskRequest  = CreateTaskRequest();
                taskRequest.Overrides.ContainerOverrides[0].Name = CONTAINER_NAME;// "BatchService";
                taskRequest.Overrides.ContainerOverrides[0].Environment.Add(new Amazon.ECS.Model.KeyValuePair() { Name = "APP_PARAM", Value = input });

                //Run the task
                AmazonECSClient ecsClient = new AmazonECSClient(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY);
                Task<RunTaskResponse> response = ecsClient.RunTaskAsync(taskRequest);

                //Collect response data
                result += BuildResponseData(response);
            }
            catch (Exception ex)
            {
                result = "Fail - msg:" + ex.Message +" stack:" + ex.StackTrace + " source:" + ex.Source + " data:" + ex.ToString();
            }

            return result;
        }

        private RunTaskRequest CreateTaskRequest()
        {
            RunTaskRequest taskRequest = new RunTaskRequest();
            taskRequest.TaskDefinition = TASK_DEFINITION;// "BatchTask:2";
            taskRequest.Cluster = CLUSTER;// "Accu-IdentityStack-TMFCOUNJA02U";
            taskRequest.LaunchType = LAUNCH_TYPE;// LaunchType.EC2;
            taskRequest.Count = COUNT;//1;

            if (taskRequest.Overrides == null)
            {
                taskRequest.Overrides = new TaskOverride();
            }
            if (taskRequest.Overrides.ContainerOverrides == null)
            {
                taskRequest.Overrides.ContainerOverrides = new List<ContainerOverride>();
            }
            if (taskRequest.Overrides.ContainerOverrides.Count == 0)
            {
                taskRequest.Overrides.ContainerOverrides.Add(new ContainerOverride());
            }
            if (taskRequest.Overrides.ContainerOverrides[0].Environment == null)
            {
                taskRequest.Overrides.ContainerOverrides[0].Environment = new List<Amazon.ECS.Model.KeyValuePair>();
            }

            return taskRequest;
        }

        //Collects response data
        private string BuildResponseData(Task<RunTaskResponse> response)
        {
            string results = " ID-" + response.Id;
            if (response.Exception != null)
            {
                results += " Exception-" + response.Exception.Message;
            }

            if (response.Result != null)
            {
                results += " HttpStatusCode-" + response.Result.HttpStatusCode + " Failure-";

                foreach (Failure ff in response.Result.Failures)
                    results += ff.Reason;
            }

            return results;
        }
    }
}
