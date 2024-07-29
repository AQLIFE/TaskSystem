using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    partial class AppFilter(ILogger<AppFilter> logger, LogInfo log) : IActionFilter
    {
        private ObjectResult? actionResult;

        public void OnActionExecuting(ActionExecutingContext action)
        {
            Console.WriteLine($"Controller name: {action.Controller.GetType().FullName}");
        }

        public void OnActionExecuted(ActionExecutedContext action)
        {
            try
            {
                ObjectResult item = action.Result as ObjectResult ?? throw new Exception(action.RouteData.Values["controller"]?.ToString() + "接口发生错误");
                Console.WriteLine(item.Value?.GetType().Name);
                actionResult = new Result<Object?>(item.Value, !(action.Exception != null || item.Value == null)).ToObjectResult();
            }
            catch (Exception)
            {
                //action.Exception = null;
                actionResult =/* GlobalResult.Message("程序错误，请联系授权");*/GlobalResult.Message(action.Exception!.ToString());
            }
            finally
            {
                log.SetMessage(action);
                logger.LogInformation(log.Message);
                action.Result = actionResult;
            }
        }
    }

    public class CustomRequirement : IAuthorizationRequirement
    {
        public int Roles => SystemInfo.AdminRole;
    }

    public class CustomHandler(ILogger<CustomHandler> logger, LogInfo log) : AuthorizationHandler<CustomRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
        {
            var userCliams = context.User.Claims;

            // 假设从Claim中获取角色ID
            if (HttpAction.GetClaim(userCliams, ClaimTypes.Role) is Claim c && Int32.TryParse(c.Value, out int num) && num >= requirement.Roles)
            {
                context.Succeed(requirement);
                log.SetMessage(context);
                logger.LogInformation(log.Message);
            }
            return Task.CompletedTask;
        }
    }

}