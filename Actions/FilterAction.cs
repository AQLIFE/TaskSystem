using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Actions
{
    public class FilterAction(ManagementSystemContext context)
    {
        // private EmployeeActions actions = new(context);
        private EmployeeActions actions = new EmployeeActions(context);
        public bool status = false;
        public static string? GetIdByClaims(IEnumerable<Claim> claims)
            => (from item in claims where item.ToString() == ClaimTypes.Authentication + ": " + item.Value select item.Value).FirstOrDefault();

        public void Cauth(ActionExecutingContext action)
        {
            if (!action.HttpContext.User.Claims.IsNullOrEmpty() && !FilterAction.GetIdByClaims(action.HttpContext.User.Claims).IsNullOrEmpty())
            {
                string id = FilterAction.GetIdByClaims(action.HttpContext.User.Claims!)!;
                EncryptAccount? obj = actions.GetEncrypts(id!);
                if (obj != null) this.VerifyPart(obj, action);
                else action.Result = GlobalResult.Cancelled;
            }
        }

        public void VerifyPart(EncryptAccount obj, ActionExecutingContext action)
        {
            if (action.ActionArguments.ContainsKey("id"))
            // 方法需要ID 且 用户权限不足90
            {
                if (obj!.AccountPermission < 90 ){
                    if( action.ActionArguments.ContainsKey("employeeSystemAccount") && action.ActionArguments["employeeSystemAccount"] is PartInfo part && part.AccountPermission >= 90)
                        action.Result = GlobalResult.LimitAuth;//无权

                    if (obj!.EncryptionId != action.ActionArguments["id"]!.ToString())
                    action.Result = GlobalResult.InvalidParameter;// 错误参数
                }
            }                                                                                                                      // 请求ID和用户ID不一
            status = true;

        }

        public LogInfo<string?> InitLog(ActionExecutingContext context)
        {
            var ob = GetIdByClaims(context.HttpContext.User.Claims);
            var oj = context.ActionDescriptor as ControllerActionDescriptor;
            return new LogInfo<string?>(status, ob, oj!.ActionName);
        }

        public LogInfo<string?> InitLog(ActionExecutedContext context)
        {
            var ob = GetIdByClaims(context.HttpContext.User.Claims);
            var oj = context.ActionDescriptor as ControllerActionDescriptor;
            var il = context.Result as ObjectResult;
            var sl = il?.Value as Result<string?>;
            return new LogInfo<string?>(status, ob, oj!.ActionName, sl?.Data);
        }
    }
}