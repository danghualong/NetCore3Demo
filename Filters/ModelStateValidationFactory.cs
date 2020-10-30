using EFTest.Configs;
using EFTest.Models;
using EFTest.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Filters
{
    public class ModelStateValidationFactory
    {
        public static IActionResult CreateModelStateActionResult(ActionContext context)
        {
            string errorMsg = GetErrorMessage(context.ModelState);
            return new JsonResult(new RespResult(BizStatusCode.ParameterError, errorMsg)) { StatusCode=400 };
        }

        private static string GetErrorMessage(ModelStateDictionary modelState,string sep="\r\n")
        {
            StringBuilder sb = new StringBuilder();
            foreach(var item in modelState)
            {
                var state = item.Value;
                string errorMsg = null;
                var error=state.Errors.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ErrorMessage));
                if (error != null)
                {
                    errorMsg = error.ErrorMessage;
                }
                else
                {
                    var ex = state.Errors.FirstOrDefault(p => p.Exception != null);
                    if (ex != null)
                    {
                        errorMsg = ex.Exception.Message;
                    }
                }
                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    continue;
                }
                if (sb.Length > 0)
                {
                    sb.Append(sep);
                }
                sb.Append(errorMsg);
            }
            return sb.ToString();
        }
    }
}
