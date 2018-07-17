using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccuMarkers.Controllers
{
    [Produces("application/json")]
    [Route("Markers")]
    public class MarkersController : Controller
    {
        private readonly log4net.ILog log = null;
        public MarkersController()
        {
            PatternLayout pattern = new PatternLayout("%date %-5level: %message%newline");
            pattern.ActivateOptions();

            ConsoleAppender appender = new ConsoleAppender();
            appender.Layout = pattern;
            appender.ActivateOptions();

            Hierarchy hierarchy = (Hierarchy)log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            hierarchy.Root.Level = Level.All;
            hierarchy.Root.AddAppender(appender);
            hierarchy.Configured = true;

            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info("Application - Main is invoked");
        }

        // GET: Markers
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string core = String.Empty;
            string host = String.Empty;
            log.Info("Application - Main is invoked xxxyyt");
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

        // GET: Markers/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: Markers
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: Markers/5
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
