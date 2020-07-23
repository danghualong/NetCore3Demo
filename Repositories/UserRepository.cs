using EFTest.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Repositories
{
    public class UserRepository
    {
        private MyContext dbContext;
        public UserRepository(MyContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> GetUserAsync(string userName,string password)
        {
            //此处密码MD5处理
            return await dbContext.Users.Where(p => string.Equals(p.UserName, userName) && string.Equals(p.Password, password)).FirstOrDefaultAsync();
        }
    }
}
