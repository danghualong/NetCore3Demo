using EFTest.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest
{
    public class DBInitializer
    {
        private MyContext dbContext;
        public DBInitializer(MyContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void InitializeDb()
        {
            if (dbContext.Users.Any())
            {
                Console.WriteLine("It has been initialized!");
                return;
            }
            var user = new User() { UserId = 1, UserName="dhl",Password="123",UserType=1 };
            dbContext.Users.Add(user);
            user = new User() { UserId = 2, UserName = "zfy", Password = "456", UserType = 2 };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var activity = new Activity() {Id=1, ActivityName = "工艺博览会", Summary = "工艺备注" };
            dbContext.Activities.Add(activity);
            activity = new Activity() { Id=2,ActivityName = "机器人大会", Summary = "机器人" };
            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            var teams = new Team() { TeamName = "中国团", ActivityId = 1, Order = 1, Summary = "主办方" };
            dbContext.Teams.Add(teams);
            teams = new Team() { TeamName = "美国团", ActivityId = 1, Order = 2, Summary = "" };
            dbContext.Teams.Add(teams);
            teams = new Team() { TeamName = "北京团", ActivityId = 2, Order = 1, Summary = "北京1" };
            dbContext.Teams.Add(teams);
            teams = new Team() { TeamName = "河北团", ActivityId = 2, Order = 2, Summary = "河北2" };
            dbContext.Teams.Add(teams);
            dbContext.SaveChanges();

            Console.WriteLine("InitializeDb Completed");
        }
    }
}
