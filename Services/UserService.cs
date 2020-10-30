using AutoMapper;
using EFTest.Models.Dtos;
using EFTest.Models.Entities;
using EFTest.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Services
{
    public class UserService
    {
        private UserRepository userRepository;
        private IMapper mapper;
        public UserService(UserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserInfo> GetUser(string userName, string password)
        {
            UserInfo user = null;
            if (string.IsNullOrEmpty(userName))
            {
                user = new UserInfo() { Id = 0, UserName = "Anonymous" };
            }
            else
            {
                var tmpUser = await userRepository.GetUserAsync(userName, password);
                if (tmpUser == null)
                {
                    return null;
                }
                //user = mapper.Map<UserInfo>(tmpUser);
                int userId = tmpUser.UserId;
                user = new UserInfo() { Id = userId, UserName = userName };
            }
            return user;
        }

        public async Task<UserInfo> Register(RegisterDto data)
        {
            var objUser = new User() { UserName = data.UserName, Password = data.Password, UserType = 1 };
            var user = await userRepository.Register(objUser);
            if (user != null)
            {
                return mapper.Map<UserInfo>(user);
            }
            return null;
        }
    }
}
