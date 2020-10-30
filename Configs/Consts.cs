using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Configs
{
    public enum BizStatusCode
    {
        [Description("成功")]
        Success = 200,
        [Description("未知异常")]
        Unknown=0, 
        [Description("当前用户再其他地方已登录")]
        Logout =601,
        [Description("用户信息不存在")]
        NoUser = 602,
        [Description("创建用户失败")]
        CreateUserFailed = 603,
        [Description("Token已过期")]
        TokenExpired = 604,
        [Description("数据参数错误")]
        ParameterError = 700,
        [Description("未授权")]
        Unauthorize = 401,
        [Description("未找到服务")]
        NoFound = 404,
        [Description("请求错误")]
        BadGateway = 502,
    }

    public enum UserType
    {
        SuperAdmin=1,
        Common=2,
        ClientAdmin=3,
    }
}
