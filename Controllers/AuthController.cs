using EFTest.Models.Dtos;
using EFTest.Repositories;
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
    [ApiController]
    public class AuthController:ControllerBase
    {
        private IConfiguration configuration;
        private UserRepository userRepository;
        public AuthController(IConfiguration configuration,UserRepository userRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
        }
        [Route("oauth/token")]
        [HttpPost]
        public async Task<ActionResult<TokenDto>> LoginAsync([FromBody] UserInfo data)
        {
            string userName = null;
            string password = null;
            if (data != null)
            {
                userName = data.UserName;
                password = data.Password;
            }
            var user = await GetUser(userName, password);
            //if (user == null)
            //{
            //    return new JsonResult(new HttpResultDto(999,"用户信息不存在"));
            //}
            var token = GetToken(user);
            return new JsonResult(new TokenDto() { Token = token, UserName = user.UserName });
        }

        private async Task<UserInfo> GetUser(string userName,string password)
        {
            UserInfo user = null;
            if (string.IsNullOrEmpty(userName))
            {
                user = new UserInfo() { Id = 0, UserName = "Anonymous" };
            }
            else
            {
                var tmpUser = await userRepository.GetUserAsync(userName, password);
                if (tmpUser==null)
                {
                    return null;
                }
                int userId = tmpUser.UserId;
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
