using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zipkin.Library;

namespace Backend_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITraceFactory _traceFactory;
        public ValuesController(ITraceFactory traceFactory)
        {
            _traceFactory = traceFactory;
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            
            using (_traceFactory.Local("Backend-Service", "Get Method example",OperationType.LocalOperation,null,true))
            {
                System.Threading.Thread.Sleep(100 * 2);

                var extraInfo = new Dictionary<string, string>();
                extraInfo.Add("service-url","http://hepsiburada.com");
                extraInfo.Add("param1", "abc");
                    
                using (_traceFactory.Local("Backend-Service", "Call external method", OperationType.LocalOperation, extraInfo,true))
                {
                    System.Threading.Thread.Sleep(100 * 3);
                    
                    extraInfo.Clear();
                    extraInfo.Add("sql-query", "select * from dbo.Customers");
                    using (_traceFactory.Local("Backend-Service", "Database-Executing query", OperationType.LocalOperation, null,true))
                    {
                        System.Threading.Thread.Sleep(100 * 1);
                    }
                }
                
                return new string[] {"value1", "value2"};
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
