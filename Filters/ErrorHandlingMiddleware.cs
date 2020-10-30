using EFTest.Configs;
using EFTest.Models;
using EFTest.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EFTest.Filters
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex.Message, ex); // 日志记录
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == (int)BizStatusCode.Unauthorize)
                {
                    msg = EnumUtil.GetDescription(statusCode,typeof(BizStatusCode));
                }
                else if (statusCode == (int)BizStatusCode.NoFound)
                {
                    msg = EnumUtil.GetDescription(statusCode, typeof(BizStatusCode));
                }
                else if (statusCode == 502)
                {
                    msg = EnumUtil.GetDescription(statusCode, typeof(BizStatusCode));
                }
                else if (statusCode != 200)
                {
                    msg = EnumUtil.GetDescription(BizStatusCode.Unknown);
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var data = new RespResult(statusCode, msg);
            var result = JsonConvert.SerializeObject(data);
            if (context.Response?.ContentType?.ToLower() == "application/xml")
            {
                await context.Response.WriteAsync(XmlConvert.Object2XmlString(data)).ConfigureAwait(false);
            }
            else
            {
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(result).ConfigureAwait(false);
            }
        }
    }
}
