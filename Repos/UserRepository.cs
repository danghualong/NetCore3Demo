using EFTest.Models.Dtos;
using EFTest.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Repos
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

        public async Task<User> Register(User user)
        {
            var tmpUser = await dbContext.Users.FirstOrDefaultAsync(p => p.UserName == user.UserName);
            if (tmpUser != null)
            {
                return null;
            }
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
    }
}
