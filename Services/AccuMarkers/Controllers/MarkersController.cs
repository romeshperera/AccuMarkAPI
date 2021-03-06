﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccuLogger;
using System.Text;

namespace AccuMarkers.Controllers
{
    [Produces("application/json")]
    [Route("Markers")]
    public class MarkersController : Controller
    {
        private log4net.ILog log = L4NLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MarkersController()
        {
            log.Info("Application - ii");
            log.Fatal("Application - ff");
            log.Error("Application - ee");
            log.Debug("Application - dd");
            log.Warn("Application - ww");
        }

        // GET: Markers
        [HttpGet]
        public IEnumerable<string> Get()
        {
            log.Info("Application - FillString");
            string markers = "";
            try
            {
                StringBuilder str = new StringBuilder(100);
                //get markers from native code
                SampleWapper.Wapper.FillString(str, str.Capacity);
                markers = str.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return markers.Split(',');


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
        public IActionResult Get(string id)
        {
            if(id == "xx" || id == "yy"|| id == "zz")
                return Ok("ID exists");
            else
                return NotFound(id);
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
