using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    partial class AppFilter(ManagementSystemContext context, ILogger<AppFilter> logger) : IActionFilter
    {
        private LogInfo<string?>? log;
        private FilterAction filter = new(context);

        public void OnActionExecuting(ActionExecutingContext action)
        {
            filter.Cauth(action);
        }

        public void OnActionExecuted(ActionExecutedContext action)
        {
            ObjectResult item = action.Result as ObjectResult ?? throw new Exception("接口发生错误");
            log = filter.InitLog(action);log.status=true;
            logger.LogInformation(log.RespenseInfomation);
            action.Result = new Result<Object?>(item.Value, !(action.Exception != null || item.Value == null)).ToObjectResult();
        }
    }
}