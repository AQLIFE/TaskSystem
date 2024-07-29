using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TaskManangerSystem.Actions
{
    public enum ParameterName { id, info, SortSerial };

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
}