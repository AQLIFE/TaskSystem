using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySql.Data.MySqlClient;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    public class APIExceptionFilter(ILogger<APIExceptionFilter> logger, DBLinkExcption log) : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                logger.LogWarning("{0}", "模型验证不通过");
                context.Result = (ObjectResult)GlobalResult.Message("模型验证不通过");
            }
            else if (context.Exception is MySqlException e)
            {
                log.SetMessage(e, context.HttpContext);

                logger.LogCritical(log.APIMessage, log.SourceID, log.SourceController, log.SourceAction, log.SourceRoute, log.TriggerTime, log.ExceptionInfo);
                context.Result = (ObjectResult)GlobalResult.Message(log.ExceptionFeedback);
            }
            //else 
            context.ExceptionHandled = true;
            await Task.CompletedTask;
        }
    }
}
