using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EFTest.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private MyContext dbContext;
        private IConfiguration configuration;
        public TeamController(MyContext dbContext,IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }
        // GET: api/Team
        [HttpGet(Name ="GetTeams")]
        [Authorize]
        public async Task<IList<Team>> GetTeams()
        {
            var list=await dbContext.Teams.ToListAsync();
            return list;
        }

        // GET: api/Team/5
        [HttpGet("{id}", Name = "GetTeam")]
        public async Task<object> GetTeam(int id)
        {
            var obj = await dbContext.Teams.Where(p => p.Id == id).Join(dbContext.Activities, t => t.ActivityId, a => a.Id, (t, a) => new { team = t, acitivity = a }).SingleOrDefaultAsync();
            return obj;
        }

        // GET: api/Team/5
        [HttpGet("name/{teamName}", Name = "GetTeamByName")]
        public object GetTeam(string teamName)
        {
            dynamic list;
            
            if (string.IsNullOrEmpty(teamName))
            {
                list = dbContext.Teams.Join(dbContext.Activities, t => t.ActivityId, a => a.Id, (t, a) => new { team = t, acitivity = a });
            }
            else
            {
                string url=Request.GetDisplayUrl();

                list = dbContext.Teams.Where(p => p.TeamName.IndexOf(teamName)>=0).Join(dbContext.Activities, t => t.ActivityId, a => a.Id, (t, a) => new { team = t, acitivity = a });
            }
            return list;
        }

        // POST: api/Team
        [HttpPost]
        public void PostTeam([FromBody] string value)
        {
        }

        // PUT: api/Team/5
        [HttpPut("{id}")]
        public void PutTeam(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteTeam(int id)
        {
        }
    }
}
