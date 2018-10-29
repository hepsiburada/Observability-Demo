using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMetrics myMetrics;

        public ValuesController(IMetrics myMet)
        {
            myMetrics = myMet;
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            myMetrics.Measure.Counter.Increment(MyMetricsRegistry.SampleCounter);
            
            using(myMetrics.Measure.Timer.Time(MyMetricsRegistry.SampleTimer, "feature-1"))
            {
                System.Threading.Thread.Sleep(250);
            }
            
            using(myMetrics.Measure.Timer.Time(MyMetricsRegistry.SampleTimer, "feature-2"))
            {
                System.Threading.Thread.Sleep(250);
            }
            
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
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
