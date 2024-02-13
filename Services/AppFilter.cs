using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{


    partial class AppFilter(ManagementSystemContext context, ILogger<AppFilter> logger) : IActionFilter
    {

        [LoggerMessage(LogLevel.Information, "{Message}")]
        public static partial void Log(ILogger logger, string Message);

        public void OnActionExecuting(ActionExecutingContext action)
        {
            if (action.HttpContext.User.Claims.IsNullOrEmpty())
            {
                string? id = (from item in action.HttpContext.User.Claims where item.ToString() == ClaimTypes.Authentication + ": " + item.Value select item.Value).FirstOrDefault();
                if (id.IsNullOrEmpty())
                {
                    EncryptAccount? obj = context.encrypts.Where(e => e.EncryptionId == id).FirstOrDefault();// 存在账户
                    if (obj != null)
                    {
                        var ol = action.ActionDescriptor as ControllerActionDescriptor;

                        // _logger.LogInformation()
                        LogInfo<string, string> log = new LogInfo<string, string>()
                        {
                            status = false,
                            name = obj.EmployeeAlias,
                            funcName = ol.ActionName,
                            invalidParameterValue = action.ActionArguments["id"]!.ToString()!,
                            parameterValue = obj!.EncryptionId
                        };
                        Log(logger, log.RequestInformation.ToString());

                        if (action.ActionArguments.ContainsKey("id") &&
                            action.ActionArguments["id"]?.ToString() != obj!.EncryptionId &&
                            obj!.AccountPermission < 90)
                            action.Result = GlobalResult.InvalidParameter;
                    }

                }
                else action.Result = GlobalResult.NoAccess;
            }
        }

        public void OnActionExecuted(ActionExecutedContext action)
        {
            HttpRequest obj = action.HttpContext.Request;
            ObjectResult item = action.Result as ObjectResult ?? throw new Exception("没有这个类型");
            action.Result = new Result<Object?>(!(action.Exception != null || item.Value == null), item.Value).ToObjectResult();
        }
    }
}