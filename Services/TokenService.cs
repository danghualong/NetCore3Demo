using EFTest.Models.Dtos;
using EFTest.Models.Entities;
using EFTest.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Services
{
    public class TokenService
    {
        private IConfiguration configuration;
        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private string GetToken(string userId,string userName, JwtSetting jwtSetting, int expireSeconds)
        {
            var curTime = DateTime.Now;
            var expireTime = curTime.AddSeconds(expireSeconds);
            var claims = new Claim[]{
                new Claim("id",userId),
                new Claim("name",userName)
            };
            var ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey));
            var sc = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jst = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                notBefore: curTime,
                expires: expireTime,
                signingCredentials: sc);
            string token = new JwtSecurityTokenHandler().WriteToken(jst);
            return token;
        }

        public TokenDto BuildToken(UserInfo user)
        {
            var jwtSetting= JwtUtil.GetJwtSetting(configuration);
            string token = GetToken(user.Id.ToString(),user.UserName, jwtSetting, jwtSetting.ExpireSeconds);
            string refreshToken = GetToken(user.Id.ToString(), user.UserName, jwtSetting,jwtSetting.RefreshExpireSeconds);
            return new TokenDto() { UserName = user.UserName,UserId=user.Id.ToString(), Token = token, RefreshToken = refreshToken };
        }
        public bool RefreshToken(TokenDto oldToken)
        {
            if (oldToken == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(oldToken.RefreshToken))
            {
                return false;
            }
            var jwtSetting = JwtUtil.GetJwtSetting(configuration);
            TokenValidationParameters paras = new TokenValidationParameters() {
                ValidIssuer = jwtSetting.Issuer,
                ValidAudience=jwtSetting.Audience,
                ValidateAudience = true,
                ValidateIssuer= true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
                ValidateLifetime = true,
                //ClockSkew=TimeSpan.Zero
            };
            SecurityToken st = null;
            try
            {
                new JwtSecurityTokenHandler().ValidateToken(oldToken.RefreshToken, paras, out st);
            }
            catch(Exception ex)
            {
                return false;
            }
            //重新更新下Token
            string token = GetToken(oldToken.UserId,oldToken.UserName, jwtSetting, jwtSetting.ExpireSeconds);
            oldToken.Token = token;
            return true;
        }
    }
}
