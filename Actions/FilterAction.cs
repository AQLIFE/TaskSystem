using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Actions
{
    public enum ParameterName { id, info, SortSerial };

    public class FilterAction(ManagementSystemContext storage, IMapper mapper)
    {
        public VaildatorForEmployee vaildatorForEmployee = new (new (storage,mapper));
        public VaildatorForCategory vaildatorForCategory = new (new (storage,mapper));
    }

    public class HttpLog<T, TResult> where T : FilterContext
    {
        public TResult InitLog(T context, Func<T, TResult> edge)
        {
            Claim? ob = context.HttpContext.User.Claims.GetClaim(ClaimTypes.Authentication);
            ControllerActionDescriptor? oj = context.ActionDescriptor as ControllerActionDescriptor;

            return edge(context);
        }
        public bool status = false;

    }

    public static class HttpAction
    {
        public static Claim? GetClaim(this IEnumerable<Claim>? claims, string flag)
            => claims?.FirstOrDefault(e => e.Type == flag);
        public static bool ExistsParmeter(this IDictionary<string, object?> pairs, string name)
            => !pairs.IsNullOrEmpty() && pairs.ContainsKey(name);

        public static bool IsAdmin(this HttpContext cx) => cx.Items.Where(e => e.Key.ToString() == "IsAdmin").FirstOrDefault().Value?.ToString() == true.ToString();

    }

    public class VaildatorForEmployee(EmployeeActions employeeAction)
    {
        public async Task<EmployeeAccount?> GetAccountByClaim(HttpContext cx, string str)
        {
            var sr = cx.User.Claims.GetClaim(str)?.Value ?? Guid.Empty.ToString();
            return await employeeAction.ExistsEmployeeByHashIdAsync(sr) ? await employeeAction.GetEmployeeByHashIdAsync(sr) : null;
        }//检查账户是否存在于令牌内并且该账户有效，如果有效则返回该账户信息

        public async Task<EmployeeAccount?> GetAccountByParameter(IDictionary<string, object?> cx, string str)
            => await employeeAction.GetEmployeeByHashIdAsync(cx[str] as string ?? string.Empty.ToString());
            //将HttpContext的请求参数字典内的指定参数 作为 员工系统的查询参数 


        public bool VerifyByEmployeeAccount(EmployeeAccount obj, HttpContext action)
                =>!action.IsAdmin() && obj.AccountPermission < SystemInfo.adminRole && obj.EmployeeAlias != SystemInfo.admin.EmployeeAlias && obj.EmployeePwd.Length >= 8;
            // 验证参数 ： 读取HttpContext 的令牌信息 ，检查是否为管理员账户 ，检查权限是否有效（AdminRole），检查账户名（不允许和管理员账户重名），检查密码（是否符合密码基础规范）


        public async Task<bool> EmployeeAccountBeForeSelect(HttpContext cx, string hashId)
        {
            EmployeeAccount? obj = await GetAccountByClaim(cx, ClaimTypes.Authentication);
            return (cx.IsAdmin() &&
            await employeeAction.ExistsEmployeeByHashIdAsync(hashId)) || (obj is EmployeeAccount nowAccount &&
            ShaHashExtensions.ComputeSHA512Hash(nowAccount.EmployeeId.ToString()) == hashId);
        }//查询前检查 ： 检查是否为有效的管理员账户 ， 检查该令牌提供的账户ID是否属于有效的ID（哈希ID）
    }

    public class VaildatorForCategory(CategoryActions categoryAction)
    {
        public async Task<bool> ValifyByMiniCate(MiniCate obj)
        {
            if (!await categoryAction.ExistsCategoryBySerialAsync(obj.ParentSortSerial) || await categoryAction.ExistsCategoryByNameAsync(obj.CategoryName)) return false;
            return true;
        }
    }

    /*public class Vaildator(ManagementSystemContext storage, IMapper mapper)
    {
        
        public static bool HttpContextParameterVerifierByCategory(this ActionExecutingContext actionHttpcontext)
        {
            IDictionary<string, object?> pairs = actionHttpcontext.ActionArguments;

            if (pairs.ExistsParmeter(ParameterName.SortSerial.ToString()))
                if (pairs.ExistsParmeter(ParameterName.info.ToString()))
                {
                    var os = pairs[ParameterName.info.ToString()] as MiniCate;
                    return os is MiniCate ? ValifyByMiniCate(os) : false;
                }
                else return pairs[ParameterName.SortSerial.ToString()] as int? >= 100;
            return true;
        }

        public async static void HttpContextParameterVerifierByEmployee(this ActionExecutingContext actionHttpcontext)
        {
            ObjectResult? result = null;
            IDictionary<string, object?> pairs = actionHttpcontext.ActionArguments;

            if (pairs.ExistsParmeter(ParameterName.id.ToString())) //是否存在ID参数
            {
                var hashId = pairs[ParameterName.id.ToString()]!.ToString()!;
                if (actionHttpcontext.HttpContext.IsAdmin() && !await employeeAction.ExistsEmployeeByHashId(hashId))
                    result = GlobalResult.NoData;
                else if (await GetAccountByClaim(actionHttpcontext.HttpContext, ClaimTypes.Authentication) is EmployeeAccount nowAccount && ShaHashExtensions.ComputeSHA512Hash(nowAccount.EmployeeId.ToString()) != hashId)
                    result = GlobalResult.InvalidParameter;

                if (pairs.ExistsParmeter(ParameterName.info.ToString()) && pairs[ParameterName.info.ToString()] is EmployeeAccountForSelectOrUpdate eAlias)
                {
                    if (await employeeAction.ExistsEmployeeByName(eAlias.EmployeeAlias) && (await employeeAction.GetEmployeeByHashId(hashId))?.EmployeeAlias != eAlias.EmployeeAlias)//是否存在info参数，并属于Ealias类型
                    {
                        result = GlobalResult.Repetition(eAlias.EmployeeAlias);
                    }


                    if (eAlias.AccountPermission >= SystemInfo.adminRole || eAlias.AccountPermission >= (await employeeAction.GetEmployeeByHashId(hashId))!.AccountPermission + 2)
                    {
                        result = GlobalResult.LimitAuth;
                    }
                }
            }

            if (result != null) actionHttpcontext.Result = result;

        }

        public static bool HttpContextParameterVerifierByTask(this ActionExecutingContext actionHttpcontext)
        {
            return true;

        }
        public static bool HttpContextParameterVerifierByCustomer(this ActionExecutingContext actionHttpcontext)
        {
            return true;
        }

    }*/
}