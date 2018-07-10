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

namespace AccuBatch.Controllers
{
    [Produces("application/json")]
    [Route("Batch")]
    public class BatchController : Controller
    {
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
            string response = "Success";
            try
            {
                AmazonECSClient ecsClient = new AmazonECSClient(Environment.GetEnvironmentVariable("KEY_ID"), Environment.GetEnvironmentVariable("KEY_SEC"));

                RunTaskRequest taskRequest = new RunTaskRequest();
                taskRequest.TaskDefinition = "BatchTask:2";
                taskRequest.LaunchType = LaunchType.EC2;
                taskRequest.Cluster = "Accu-IdentityStack-TMFCOUNJA02U";
                taskRequest.Count = 1;

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
                taskRequest.Overrides.ContainerOverrides[0].Name = "BatchService";
                taskRequest.Overrides.ContainerOverrides[0].Environment.Add(new Amazon.ECS.Model.KeyValuePair() { Name = "AWS_REGION", Value = input });


                Task<RunTaskResponse> tasks = ecsClient.RunTaskAsync(taskRequest);

                response += " ID-" + tasks.Id;
                if (tasks.Exception != null)
                {
                    response += " Ex-" + tasks.Exception.Message;
                }

                if (tasks.Result != null)
                {
                    response += " Res-" + tasks.Result.HttpStatusCode + " Fcode-";

                    foreach (Failure ff in tasks.Result.Failures)
                        response += ff.Reason;
                }
            }
            catch (Exception ex)
            {
                response = "MAIN Fail-" + ex.Message + ex.StackTrace + ex.Source + ex.ToString();
            }

            return response;
        }
    }
}
