using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Controllers;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    partial class AppFilter(ManagementSystemContext management, ILogger<AppFilter> logger) : IActionFilter /* ,IAuthorizationFilter */
    {
        private LogInfo<string?>? log;
        private FilterAction filterAction = new(management);

        public void OnActionExecuting(ActionExecutingContext action)
        {
            filterAction.pairs = action.ActionArguments;
            
            switch (action.Controller.GetType().FullName)
            {
                case string s when s == typeof(EmployeeController).FullName:    if (filterAction.ParameterVerifierByEmployee(action.HttpContext) == false)  action.Result = GlobalResult.NoData; break;
                case string s when s == typeof(CategoryController).FullName:    if (filterAction.ParameterVerifierByCategory() == false)                    action.Result = GlobalResult.NoData; break;
                case string s when s == typeof(TaskSystemController).FullName:  if (filterAction.ParameterVerifierByTask() == false)                        action.Result = GlobalResult.NoData; break;
                case string s when s == typeof(CustomerController).FullName:    if (filterAction.ParameterVerifierByCustomer() == false)                    action.Result = GlobalResult.NoData; break;
            }

            // 输出或记录控制器名称
            Console.WriteLine($"Controller name: {action.Controller.GetType().FullName}");
        }

        public void OnActionExecuted(ActionExecutedContext action)
        {
            ObjectResult item = action.Result as ObjectResult ?? throw new Exception(action.RouteData.Values + "接口发生错误");
            log = filterAction.InitLog(action); log.status = true;
            logger.LogInformation(log.RespenseInfomation);
            action.Result = new Result<Object?>(item.Value, !(action.Exception != null || item.Value == null)).ToObjectResult();
        }
    }
    public class CustomRequirement : IAuthorizationRequirement
    {
        public int Roles => SystemInfo.adminRole;
    }

    public class CustomHandler : AuthorizationHandler<CustomRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
        {
            var userCliams = context.User.Claims;

            // 假设从Claim中获取角色ID
            if (FilterAction.GetClaim(userCliams, ClaimTypes.Role) is Claim c && Int32.TryParse(c.Value, out int num) && num >= requirement.Roles) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

}