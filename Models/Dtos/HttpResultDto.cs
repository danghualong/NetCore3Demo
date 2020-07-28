using EFTest.Configs;
using EFTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Models.Dtos
{
    public class HttpResultDto<T>
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public  T Content { get; set; }
        public HttpResultDto(T content) 
        {
            this.Code = (int)BizStatusCode.Success;
            this.Content = content;
        }
        public HttpResultDto()
        {
            this.Code= (int)BizStatusCode.Success;
        }
    }

    public class HttpResultDto : HttpResultDto<string>
    {
        /// <summary>
        /// 有错误状态码
        /// </summary>
        /// <param name="errorCode"></param>
        public HttpResultDto(BizStatusCode errorCode) : this(errorCode, string.Empty)
        {
        }
        /// <summary>
        /// 有错误状态码和错误信息
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public HttpResultDto(BizStatusCode errorCode, string errorMessage)
        {
            this.Code = (int)errorCode;
            this.Error = errorMessage;
            //未指定错误消息
            if (string.IsNullOrEmpty(errorMessage))
            {
                string codeMsg = EnumUtil.GetDescription(errorCode);
                if (!string.IsNullOrEmpty(codeMsg))
                {
                    this.Error = codeMsg;
                }
            }
        }
    }
}
