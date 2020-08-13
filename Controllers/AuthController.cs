using AutoMapper;
using EFTest.Models.Dtos;
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
        private IMapper mapper;
        private IConfiguration configuration;
        private UserService userService;
        private TokenService tokenService;
        public AuthController(IConfiguration configuration,IMapper mapper,
            TokenService tokenService,UserService userService)
        {
            this.mapper = mapper;
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
                return new JsonResult(new HttpResultDto(Configs.BizStatusCode.NoUser));
            }
            var objToken = tokenService.BuildToken(user);
            return new JsonResult(new HttpResultDto<TokenDto>(objToken));
        }
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<TokenDto>> RegisterAsync([FromBody] RegisterDto data)
        {
            var user = await userService.Register(data);
            if (user != null)
            {
                var userInfo= mapper.Map<UserInfo>(user);
                return new JsonResult(new HttpResultDto<UserInfo>(userInfo));
            }
            else
            {
                return new JsonResult(new HttpResultDto(Configs.BizStatusCode.CreateUserFailed));
            }
        }
        [Route("oauth/refresh")]
        [HttpPost]
        public async Task<ActionResult<TokenDto>> RefreshTokenAsync([FromBody] TokenDto data)
        {
            var result=tokenService.RefreshToken(data);
            if (result)
            {
                return new JsonResult(new HttpResultDto<TokenDto>(data));
            }
            else
            {
                return new JsonResult(new HttpResultDto(Configs.BizStatusCode.TokenExpired));
            }
            
        }
    }
}
