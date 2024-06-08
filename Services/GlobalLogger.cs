using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TaskManangerSystem.Actions;

namespace TaskManangerSystem.Services
{
    public class LogInfo
    {
        public bool status { set; get; }

        public string? name { set; get; }

        public string funcName { set; get; } = string.Empty;

        // public T? invalidParameterValue { set; get; }

        public LogInfo() { }

        public void SetMessage(ActionExecutedContext context, bool s = true)
        {
            this.status = s;
            this.funcName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName ?? "Error Func";
            this.name = context.HttpContext.User.Claims.GetClaim(ClaimTypes.Authentication)?.Value;
        }

        public void SetMessage(AuthorizationHandlerContext context, bool s = true)
        {
            this.status = s;
            this.name = context.User.Claims.GetClaim(ClaimTypes.Authentication)?.Value;
            this.funcName = context.GetType().Name;
        }


        public string Message => $"Status > {this.status} ;Func > {this.funcName} ; TokenId > {this.name}";



        // public string RequestInformation => $"访问状态：{(status ? "succeed" : "fail")},时间：{DateTime.Now},请求用户:{name ?? "未知用户"},请求方法:{funcName}";

        // public string RespenseInfomation => $"API Status: {(status ? "succeed" : "fail")},时间：{DateTime.Now},请求用户:{name ?? "匿名用户"},请求方法:{funcName}";
    }

}