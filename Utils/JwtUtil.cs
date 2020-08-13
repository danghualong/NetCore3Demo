using EFTest.Models.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Utils
{
    public class JwtUtil
    {
        public static JwtSetting  GetJwtSetting(IConfiguration configuration)
        {
            var jwtSetting = new JwtSetting();
            jwtSetting.Audience = configuration["JwtSettings:Audience"];
            jwtSetting.Issuer = configuration["JwtSettings:Issuer"];
            jwtSetting.SecretKey = configuration["JwtSettings:SecretKey"];
            string strSeconds = configuration["JwtSettings:ExpireSeconds"];
            int expireSeconds = 0;
            int.TryParse(strSeconds, out expireSeconds);
            jwtSetting.ExpireSeconds = expireSeconds <= 0 ? 300 : expireSeconds;
            strSeconds = configuration["JwtSettings:RefreshExpireSeconds"];
            expireSeconds = 0;
            int.TryParse(strSeconds, out expireSeconds);
            jwtSetting.RefreshExpireSeconds = expireSeconds <= 0 ? 86400 : expireSeconds;
            return jwtSetting;
        }
    }
}
