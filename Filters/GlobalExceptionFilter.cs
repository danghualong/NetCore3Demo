using EFTest.Configs;
using EFTest.Models;
using EFTest.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                //http状态码是500
                context.Result = new JsonResult(new RespResult(BizStatusCode.Unknown)) 
                {StatusCode=500};
            }
            context.ExceptionHandled = true;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
