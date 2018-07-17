using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccuMarkers.Controllers
{
    [Produces("application/json")]
    [Route("Markers")]
    public class MarkersController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MarkersController()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(System.IO.File.OpenRead("log4net.config"));

            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            log.Info("Application - Main is invoked");
        }

        // GET: Markers
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string core = String.Empty;
            string host = String.Empty;
            log.Info("Application - Main is invoked xxx");
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
