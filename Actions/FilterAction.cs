using System.Runtime.InteropServices;
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
        private EmployeeActions employeeAction = new EmployeeActions(context);
        public bool status = false;

        [Obsolete("Please use GetClaim")]
        public static string? GetIdByClaims(IEnumerable<Claim> claims)
            => (from item in claims where item.ToString() == ClaimTypes.Authentication + ": " + item.Value select item.Value).FirstOrDefault();


        public EmployeeAccount? GetAccountByClaim(HttpContext cx, string str)
        {
            var sr = GetClaim(cx.User.Claims, str)?.Value ?? Guid.Empty.ToString();
            return employeeAction.ExistsEmployeeByHashId(sr) ? employeeAction.GetEmployeeByHashId(sr) : null;
        }

        public EmployeeAccount? GetAccountByParameter(ActionExecutingContext cx, string str)
            => employeeAction.GetEmployee(GetParameter(cx.ActionArguments, str) as string ?? Guid.Empty.ToString());



        public static Claim? GetClaim(IEnumerable<Claim>? claims, string flag)
            => claims?.FirstOrDefault(e => e.Type == flag);

        public static bool ExistsParmeter(IDictionary<string, object?> pairs, string name) => !pairs.IsNullOrEmpty() && pairs.ContainsKey(name);
        public static object? GetParameter(IDictionary<string, object?> pairs, string name)
            => pairs[name];

        public bool Validators(Claim kv, string obj) => kv.Value == obj;


        [Obsolete]
        public void VerifyPart(EmployeeAccount obj, ActionExecutingContext action)
        {
            if (action.ActionArguments.ContainsKey("id"))
            // 方法需要ID 且 用户权限不足90
            {
                if (obj!.AccountPermission < 90)
                {
                    if (action.ActionArguments.ContainsKey("employeeSystemAccount") && action.ActionArguments["employeeSystemAccount"] is PartInfo part && part.AccountPermission >= 90)
                        action.Result = GlobalResult.LimitAuth;//无权

                    if (obj.EmployeeId.ToString() != action.ActionArguments["id"]!.ToString())
                        action.Result = GlobalResult.InvalidParameter;// 错误参数
                }

            }                                                                                                                      // 请求ID和用户ID不一
            status = true;

        }

        public LogInfo<string?> InitLog(ActionExecutingContext context)
        {
            var ob = GetClaim(context.HttpContext.User.Claims, ClaimTypes.Authentication);
            var oj = context.ActionDescriptor as ControllerActionDescriptor;
            return new LogInfo<string?>(status, ob?.Value, oj!.ActionName);
        }

        public LogInfo<string?> InitLog(ActionExecutedContext context)
        {
            var ob = GetClaim(context.HttpContext.User.Claims, ClaimTypes.Authentication);
            var oj = context.ActionDescriptor as ControllerActionDescriptor;
            var il = context.Result as ObjectResult;
            var sl = il?.Value as Result<string?>;
            return new LogInfo<string?>(status, ob?.Value, oj!.ActionName, sl?.Data?.GetType().Name);
        }
    }
}