using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TaskManangerSystem.Actions;

namespace TaskManangerSystem.Services
{
    public class CustomRequirement : IAuthorizationRequirement
    {
        public int Roles => SystemInfo.AdminRole;
    }

    public class CustomHandler(ILogger<CustomHandler> logger, APILog log) : AuthorizationHandler<CustomRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
        {
            var userCliams = context.User.Claims;

            // 假设从Claim中获取角色ID
            if (HttpAction.GetClaim(userCliams, ClaimTypes.Role) is Claim c && Int32.TryParse(c.Value, out int num) && num >= requirement.Roles)
            {
                context.Succeed(requirement);
                log.SetMessage(context);
                logger.LogInformation(log.APIMessage);
            }
            await Task.CompletedTask;
        }
    }
}
