using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        // GET: api/Activity
        [HttpGet]
        public IEnumerable<string> GetActivities()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Activity/5
        [HttpGet("{id}", Name = "Get")]
        public string GetActivity(int id)
        {
            return "value";
        }

        // POST: api/Activity
        [HttpPost]
        public void PostActivity([FromBody] string value)
        {
        }

        // PUT: api/Activity/5
        [HttpPut("{id}")]
        public void PutActivity(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteActivity(int id)
        {
        }
    }
}
