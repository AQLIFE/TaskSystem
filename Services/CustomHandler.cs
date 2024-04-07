namespace TaskManangerSystem.Services
{
    // public class CustomRequirement : IAuthorizationRequirement
    // {
    //     public int Roles => SystemInfo.adminRole;
    // }

    // public class CustomHandler : AuthorizationHandler<CustomRequirement>
    // {
    //     protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
    //     {
    //         var userCliams = context.User.Claims;

    //         // 假设从Claim中获取角色ID
    //         if (FilterAction.GetClaim(userCliams, ClaimTypes.Role) is Claim c && Int32.TryParse(c.Value, out int num) && num >= requirement.Roles) context.Succeed(requirement);

    //         return Task.CompletedTask;
    //     }
    // }
}