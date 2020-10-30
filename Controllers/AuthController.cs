using AutoMapper;
using EFTest.Models.Dtos;
using EFTest.Models;
using EFTest.Models.Entities;
using EFTest.Repos;
using EFTest.Services;
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
        private UserService userService;
        private TokenService tokenService;
        public AuthController(IConfiguration configuration,
            TokenService tokenService,UserService userService)
        {
            this.configuration = configuration;
            this.tokenService = tokenService;
            this.userService = userService;
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
            var user = await userService.GetUser(userName, password);
            if (user == null)
            {
                return new JsonResult(new RespResult(Configs.BizStatusCode.NoUser));
            }
            var objToken = tokenService.BuildToken(user);
            return new JsonResult(new RespResult<TokenDto>(objToken));
        }
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<TokenDto>> RegisterAsync([FromBody] RegisterDto data)
        {
            var userInfo = await userService.Register(data);
            if (userInfo != null)
            {
                return new JsonResult(new RespResult<UserInfo>(userInfo));
            }
            else
            {
                return new JsonResult(new RespResult(Configs.BizStatusCode.CreateUserFailed));
            }
        }
        [Route("oauth/refresh")]
        [HttpPost]
        public async Task<ActionResult<TokenDto>> RefreshTokenAsync([FromBody] TokenDto data)
        {
            var result=tokenService.RefreshToken(data);
            if (result)
            {
                return new JsonResult(new RespResult<TokenDto>(data));
            }
            else
            {
                return new JsonResult(new RespResult(Configs.BizStatusCode.TokenExpired));
            }
            
        }
    }
}
