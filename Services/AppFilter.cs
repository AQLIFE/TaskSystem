
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    partial class AppFilter(ManagementSystemContext context, ILogger<AppFilter> logger) : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext action)
        {
            if (!action.HttpContext.User.Claims.IsNullOrEmpty() && !GetIdByClaims(action.HttpContext.User.Claims).IsNullOrEmpty())
            {
                string id = GetIdByClaims(action.HttpContext.User.Claims!)!;
                EncryptAccount? obj = GetEncryptAccount(id!);
                if (obj != null) VerifyPart(obj, action);
                else action.Result = GlobalResult.Cancelled;
            }
        }

        private EncryptAccount? GetEncryptAccount(string id) => context.encrypts.Where(e => e.EncryptionId == id).FirstOrDefault();

        private bool EmployeeSystemAccountExists(string name)=>context.encrypts.Any(e=>e.EmployeeAlias==name);

        private string? GetIdByClaims(IEnumerable<Claim> claims)
            => (from item in claims where item.ToString() == ClaimTypes.Authentication + ": " + item.Value select item.Value).FirstOrDefault();

        private void VerifyPart(EncryptAccount obj, ActionExecutingContext action)
        {
            if (action.ActionArguments.ContainsKey("id") &&
                obj!.AccountPermission < 90)
            // 方法需要ID 且 用户权限不足90
            {
                if (action.ActionArguments.ContainsKey("employeeSystemAccount") && action.ActionArguments["employeeSystemAccount"] is PartInfo part)
                {
                    if (part.AccountPermission >= 90) action.Result = GlobalResult.LimitAuth; //无权
                    else if( EmployeeSystemAccountExists(part.EmployeeAlias) )action.Result = GlobalResult.Repetition(part.EmployeeAlias);
                    // else if( context.encrypts.Any(e=>e.EmployeePwd!=part.EmployeePwd))action.Result = GlobalResult.PWDError;
                }
                // 请求ID和用户ID不一致
                if (obj!.EncryptionId != action.ActionArguments["id"]!.ToString()) action.Result = GlobalResult.InvalidParameter;// 错误参数
            }
            var oj = action.ActionDescriptor as ControllerActionDescriptor;

            Console.WriteLine($"{oj.ActionName}");
        }

        public void OnActionExecuted(ActionExecutedContext action)
        {
            HttpRequest obj = action.HttpContext.Request;
            ObjectResult item = action.Result as ObjectResult ?? throw new Exception("没有这个类型");
            action.Result = new Result<Object?>(item.Value,!(action.Exception != null || item.Value == null)).ToObjectResult();
        }
    }
}