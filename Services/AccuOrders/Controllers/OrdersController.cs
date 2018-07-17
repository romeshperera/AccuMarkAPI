using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using log4net;
using System.Xml;
using System.Reflection;
using Amazon;
using log4net.Core;
using AWS.Logger.Log4net;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using Amazon.Runtime;

namespace AccuOrders.Controllers
{
    [Produces("application/json")]
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly string AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        private readonly string AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

        ILog log = LogManager.GetLogger(typeof(OrdersController));
        public OrdersController()
        {
            ConfigureLog4net();

           
            log.Info("Check the AWS Console CloudWatch Logs console in us-east-1");
            log.Info("to see messages in the log streams for the");
            log.Info("log group Log4net.ProgrammaticConfigurationExample");
        }

        private void ConfigureLog4net()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository("OrdersController");
            PatternLayout patternLayout = new PatternLayout();

            patternLayout.ConversionPattern = "%-4timestamp [%thread] %-5level %logger %ndc - %message%newline";
            patternLayout.ActivateOptions();

            AWSAppender appender = new AWSAppender();
            appender.Layout = patternLayout;
            appender.Credentials = new BasicAWSCredentials(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY);

            // Set log group and region. Assume credentials will be found using the default profile or IAM credentials.
            appender.LogGroup = "Log4net.ProgrammaticConfigurationExample";
            appender.Region = "us-west-2";

            appender.ActivateOptions();
            hierarchy.Root.AddAppender(appender);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        // GET: Orders
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string core = String.Empty;
            string host = String.Empty;
            log.Info("xxxxxxxxxxxxxx");
            Console.WriteLine("yyyyyyyyyy");
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

            return new string[] { "xCppWapper: " + core, "HostIP: " + host };
        }

        // GET: Orders/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            //Console.WriteLine("Got the requst for - " + id);
            //_logger.LogInformation(LoggingEvents.GetItem, "Getting item {ID}", id);
            if (id == 0)
            {
                //Console.Error.WriteLine("Sample Error for - " + id);
                //_logger.LogWarning(LoggingEvents.GetItemNotFound,"GetById({ID}) NOT FOUND", id);
                return NotFound("Invalid ID");
            }
            if (id == 1)
            {
                Console.Error.WriteLine("Sample Error for - " + id);
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception ex)
                {
                    //_logger.LogWarning(LoggingEvents.GetItemNotFound, ex, "GetById({ID}) NOT FOUND", id);
                    return NotFound();
                }
                
                //return NotFound("Invalid ID");
            }
            if (id == 100)
            {
                Console.Error.WriteLine("Sample Error for - " + id);
                return NoContent();
            }
            return Ok("valuexxx");
        }
        
        // POST: Orders
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: Orders/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
