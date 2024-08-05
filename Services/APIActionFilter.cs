using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    partial class APIActionFilter(ILogger<APIActionFilter> logger, APILog log) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult("输入信息不符规范");

            ActionExecutedContext edContext = await next();

            ObjectResult? item = edContext.Result as ObjectResult;


            log.SetMessage(context);
            logger.LogInformation(log.APIMessage, log.SourceID, log.SourceController, log.SourceAction, log.SourceRoute, log.TriggerTime);
            edContext.Result = (ObjectResult)GlobalResult.Succeed(item?.Value);
        }
    }

}