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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class FilterAction(ManagementSystemContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        private EmployeeActions employeeAction = new(context);
        private CategoryActions categoryActions = new(context);

        public bool status = false;

        public EmployeeAccount? GetAccountByClaim(HttpContext cx, string str)
        {
            var sr = GetClaim(cx.User.Claims, str)?.Value ?? Guid.Empty.ToString();
            return employeeAction.ExistsEmployeeByHashId(sr) ? employeeAction.GetEmployeeByHashId(sr) : null;
        }

        public EmployeeAccount? GetAccountByParameter(IDictionary<string, object?> cx, string str)
            => employeeAction.GetEmployeeByHashId(cx[str] as string ?? string.Empty.ToString());



        public static Claim? GetClaim(IEnumerable<Claim>? claims, string flag)
            => claims?.FirstOrDefault(e => e.Type == flag);

        public static bool ExistsParmeter(IDictionary<string, object?> pairs, string name) => !pairs.IsNullOrEmpty() && pairs.ContainsKey(name);

        // public bool Validators(Claim kv, string obj) => kv.Value == obj;


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

        public enum ParameterName { id, info, SortSerial };

        public IDictionary<string, object?> pairs;
        public bool ParameterVerifierByCategory()
        {
            // 序列号验证器
            if (ExistsParmeter(pairs, ParameterName.SortSerial.ToString()))
                if (ExistsParmeter(pairs, ParameterName.info.ToString()))
                {
                    var os = pairs[ParameterName.info.ToString()] as MiniCate;
                    return os is MiniCate ? ValifyByMiniCate(os) : false;
                }
                else return pairs[ParameterName.SortSerial.ToString()] as int? >= 100;
            return true;
        }

        public bool ParameterVerifierByEmployee(HttpContext cx)
        {
            bool status = true;
            if (ExistsParmeter(pairs, ParameterName.id.ToString()))
            {
                var obj =GetAccountByParameter(pairs, ParameterName.id.ToString());
                status = obj is EmployeeAccount;
                // var os = GetAccountByParameter(pairs, ParameterName.id.ToString()) is EmployeeAccount obj;

                if (status && ExistsParmeter(pairs, ParameterName.info.ToString()))
                    return VerifyByPartInfo(obj!, cx);
                else return status;
            }
            return status;

        }
        public bool ParameterVerifierByTask()
        {
            return true;

        }
        public bool ParameterVerifierByCustomer()
        {
            return true;

        }

        public bool VerifyByPartInfo(EmployeeAccount obj, HttpContext action)
        => !IsAdmin(action) && obj.AccountPermission < SystemInfo.adminRole && obj.EmployeeAlias != SystemInfo.admin.EmployeeAlias && obj.EmployeePwd.Length >= 8;

        public bool ValifyByMiniCate(MiniCate obj)
        {
            if (!categoryActions.ExistsCategoryBySerial(obj.ParentSortSerial) || categoryActions.ExistsCategoryByName(obj.CategoryName)) return false;
            return true;
        }
        public bool IsAdmin(HttpContext cx) => cx.Items.Where(e => e.Key.ToString() == "IsAdmin").FirstOrDefault().Value?.ToString() == true.ToString();
    }
}