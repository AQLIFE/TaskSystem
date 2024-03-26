using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    public class CustomRoleRequirement(int role=1) : IAuthorizationRequirement
    {
        public int Roles { get; }=role;
    }

    public class CustomRoleHandler : AuthorizationHandler<CustomRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRoleRequirement requirement)
        {
            var userCliams = context.User.Claims;

            // 假设从Claim中获取角色ID
            if (FilterAction.GetClaim(userCliams,ClaimTypes.Role) is Claim c && Int32.TryParse(c.Value,out int num) && num >= requirement.Roles)context.Succeed(requirement);
            
            return Task.CompletedTask;
        }
    }
    // public class AdminAuthorizeAttribute(int role = 1) : AuthorizeAttribute, IActionFilter/* IAuthorizationFilter */
    // {
    //     private new int Roles { set; get; } = role;
    //     // public FilterAction? filterAction = new(management);

    //     public void OnActionExecuting(ActionExecutingContext actionExecuting)
    //     {
    //         if (actionExecuting == null) throw new ArgumentNullException(nameof(actionExecuting));


    //         // 检查AccessThreshold的值
    //         // if ( Roles> filterAction?.GetAccountByClaim(context.HttpContext, ClaimTypes.Authentication)?.AccountPermission)

    //         if (FilterAction.ExistsParmeter(actionExecuting.ActionArguments, "id") && FilterAction.GetParameter(actionExecuting.ActionArguments, "id") is string str)
    //         {

    //         }

    //     }
    //     public void OnActionExecuted() { }

    //     public bool OnEmployeeId(IEnumerable<Claim> claims)
    //     {
    //         string? claimValue = FilterAction.GetClaim(claims, ClaimTypes.Role)?.Value;
    //         int n = int.TryParse(claimValue, out int num) ? num : 1;
    //         return n < Roles;


    //     }

    //     public void OnPart() { }
    //     public void OnPartInfo() { }
    // }
}