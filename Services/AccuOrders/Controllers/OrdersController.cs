﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AccuLogger; 

namespace AccuOrders.Controllers
{
    [Produces("application/json")]
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly ILogger _logger;

        public OrdersController(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("AccuOrders.Controllers.OrdersController");
        }

        // GET: Orders
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

        // GET: Orders/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            //Console.WriteLine("Got the requst for - " + id);
            _logger.LogInformation(LoggingEvents.GetItem, "Getting item {ID}", id);
            if (id == 0)
            {
                //Console.Error.WriteLine("Sample Error for - " + id);
                _logger.LogWarning(LoggingEvents.GetItemNotFound,"GetById({ID}) NOT FOUND", id);
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
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, ex, "GetById({ID}) NOT FOUND", id);
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
