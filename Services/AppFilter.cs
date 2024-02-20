
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    partial class AppFilter(ManagementSystemContext context, ILogger<AppFilter> logger) : IActionFilter
    {
        private LogInfo<string?> log;
        private FilterAction filter = new(context);

        public void OnActionExecuting(ActionExecutingContext action)
        {
            filter.Cauth(action);
            Console.WriteLine("Filter 正常");
        }

        public void OnActionExecuted(ActionExecutedContext action)
        {
            ObjectResult item = action.Result as ObjectResult ?? throw new Exception("没有这个类型");
            log = filter.InitLog(action);log.status=true;
            logger.LogInformation(log.RespenseInfomation);
            action.Result = new Result<Object?>(item.Value, !(action.Exception != null || item.Value == null)).ToObjectResult();
        }
    }
}