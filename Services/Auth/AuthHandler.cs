﻿using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Tool;

namespace TaskManangerSystem.Services.Auth
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
            if (userCliams.GetClaim(ClaimTypes.Role) is Claim c && int.TryParse(c.Value, out int num) && num >= requirement.Roles)
            {
                context.Succeed(requirement);
                log.SetMessage(context);
                logger.LogInformation(log.APIMessage, log.SourceID, log.SourceController, log.SourceAction, log.SourceRoute, log.TriggerTime);
            }
            await Task.CompletedTask;
        }
    }
}
