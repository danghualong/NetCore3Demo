using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFTest.Filters;
using EFTest.Repositories;
using EFTest.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
            services.AddControllers(option=>
            {
                option.Filters.Add<GlobalExceptionFilter>();
            });
            services.AddDbContext<MyContext>((optionsBuilder) =>
            {
                optionsBuilder.UseSqlite(Configuration.GetConnectionString("db"));
            });
            services.AddTransient<UserRepository>();
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
                        //添加此属性过期时间才生效
                        //ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddCors(options =>
            {
                options.AddPolicy("any",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                        .AllowAnyOrigin().AllowAnyMethod();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,MyContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("any");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            new DBInitializer(dbContext).InitializeDb();
        }
    }
}
