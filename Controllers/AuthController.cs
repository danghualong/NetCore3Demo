using EFTest.Models.Dtos;
using EFTest.Models.Entities;
using EFTest.Utils;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private IConfiguration configuration;
        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost]
        public async Task<string> LoginAsync([FromBody] UserInfo data)
        {
            await Task.CompletedTask;
            string userName = null;
            string password = null;
            if (data == null)
            {
                userName = data.UserName;
                password = data.Password;
            }
            
            var user = GetUser(userName, password);
            string token = GetToken(user);
            return token;
        }

        private UserInfo GetUser(string userName,string password)
        {
            UserInfo user = null;
            if (string.IsNullOrEmpty(userName))
            {
                user = new UserInfo() { Id = 0, UserName = "Anonymous" };
            }
            else
            {
                //查询数据库，获取UserId
                
                int userId = 100;
                user = new UserInfo() { Id = userId, UserName = userName };
            }
            return user;
        }

        private string GetToken(UserInfo user)
        {
            var jwtSetting = JwtUtil.GetJwtSetting(configuration);
            var curTime = DateTime.Now;
            var expireTime= curTime.AddSeconds(jwtSetting.ExpireSeconds);
            var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("id",user.Id.ToString()),
                new Claim("name",user.UserName),
                new Claim("isAdmin",user.IsAdmin.ToString())
            };
            
            var ssk= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey));
            var sc = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jst = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                notBefore: curTime,
                expires: expireTime,
                signingCredentials: sc) ;
            string token=new JwtSecurityTokenHandler().WriteToken(jst);
            return token;
        }
    }
}
