using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    partial class AppFilter(ManagementSystemContext management, ILogger<AppFilter> logger) :IActionFilter /* ,IAuthorizationFilter */
    {
        private LogInfo<string?>? log;
        private FilterAction filterAction = new(management);

        // private EmployeeAccount? account;
        // bool IsSuccess = false;
        // private int Roles  =1;

        // private bool IsFlag(HttpContext httpContext, string fullName)
        //     => httpContext.GetEndpoint()?.Metadata.Any(e => e.GetType().FullName == fullName) ?? false;
        
        // private bool IsAllowAnonymous(HttpContext httpContext)
        //     => IsFlag(httpContext, typeof(AllowAnonymousAttribute).FullName!);

        // private bool IsAuthorization(HttpContext httpContext)
        //     => IsFlag(httpContext, typeof(AuthorizeAttribute).FullName!);

        // public void OnAuthorization(AuthorizationFilterContext authFilter)
        // {
        //     // if (!IsAllowAnonymous(authFilter.HttpContext) && IsAuthorization(authFilter.HttpContext))
        //     // {
        //     //     // 检查是否具有JWT
        //     //     // account = obj is not null? employeeAction.GetEmployee(obj.Value):null;
        //     //     account = filterAction.GetAccountByClaim(authFilter.HttpContext, ClaimTypes.Authentication);
                
        //     //     //检查JWT的ID所对应的权限
        //     //     if ((account?.AccountPermission ?? 0) < Roles) authFilter.Result = new ObjectResult(GlobalResult.Message($"全局规则:权限等级不足"));
        //     // }
        // }


        public void OnActionExecuting(ActionExecutingContext action){}

        public void OnActionExecuted(ActionExecutedContext action)
        {
            ObjectResult item = action.Result as ObjectResult ?? throw new Exception(action.RouteData.Values + "接口发生错误");
            log = filterAction.InitLog(action); log.status = true;
            logger.LogInformation(log.RespenseInfomation);
            action.Result = new Result<Object?>(item.Value, !(action.Exception != null || item.Value == null)).ToObjectResult();
        }
    }
}