using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EFTest.Filters;
using EFTest.Models;
using EFTest.Models.Dtos;
using EFTest.Repos;
using EFTest.Services;
using EFTest.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EFTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //������֤ʧ�ܷ��ؽ��
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    //StringBuilder errTxt = new StringBuilder();
                    //foreach (var item in context.ModelState.Values)
                    //{
                    //    foreach (var error in item.Errors)
                    //    {
                    //        errTxt.Append(error.ErrorMessage + "|");
                    //    }
                    //}

                    ////ApiResp result = new ApiResp(ApiRespCode.F400000, errTxt.ToString().Substring(0, errTxt.Length - 1));
                    //return new JsonResult(new { Errors = errTxt.ToString().Substring(0, errTxt.Length - 1) });
                    return ModelStateValidationFactory.CreateModelStateActionResult(context);
                };
            });
            //�������ݿ�������Ϣ
            services.AddDbContext<MyContext>((optionsBuilder) =>
            {
                optionsBuilder.UseSqlite(Configuration.GetConnectionString("db"));
            });
            //����ȫ���쳣������
            //���ʹ���쳣�����м����������ù�����
            services.AddControllers(options =>
            {
                //options.Filters.Add<GlobalExceptionFilter>();
            });
            services.AddScoped<UserRepository>();
            services.AddScoped<UserService>();
            services.AddScoped<TokenService>();
            //����Token��֤����
            var jwtSetting = JwtUtil.GetJwtSetting(Configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((options) =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSetting.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSetting.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
                        ValidateLifetime = true,
                        //��Ӵ����Թ���ʱ�����Ч
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        //OnAuthenticationFailed = (context) =>
                        //{
                        //    //if (typeof(SecurityTokenExpiredException) == context.Exception.GetType())
                        //    //{
                        //    //    context.Response.Headers.Add("reason", "token expired");
                        //    //}
                        //    //return Task.CompletedTask;
                        //},
                        //Token��֤ʧ�ܵĻص��¼�
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            var content=JsonConvert.SerializeObject(new RespResult(Configs.BizStatusCode.TokenExpired));
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            context.Response.ContentType = "application/json";
                            context.Response.WriteAsync(content);
                            return Task.CompletedTask;
                        }
                    };
                });
            //��������������
            services.AddCors(options =>
            {
                options.AddPolicy("any",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                        .AllowAnyOrigin().AllowAnyMethod();
                    });
            });
            //����entity��dto��ӳ�����
            services.AddAutoMapper(this.GetType().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,MyContext dbContext)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("any");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //��ʼ������
            new DBInitializer(dbContext).InitializeDb();
        }
    }
}
