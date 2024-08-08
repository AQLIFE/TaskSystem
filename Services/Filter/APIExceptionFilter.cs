using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySql.Data.MySqlClient;
using TaskManangerSystem.Services.Attriabute;
using TaskManangerSystem.Services.Info;

namespace TaskManangerSystem.Services.Filter
{
    public class APIExceptionFilter(ILogger<APIExceptionFilter> logger, DBLinkExcptionLog log) : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var x = context.ModelState.Values.First().Errors.First().ErrorMessage;
                logger.LogWarning("[ 模型验证失败 ] : {0}", x);
                context.Result = (ObjectResult)GlobalResult.Message("模型验证不通过");
            }
            else if (context.Exception is FormatException ex)
            {
                logger.LogWarning("{0} : {1}", ex.GetType().Name, ex.Message);
                context.Result = (ObjectResult)GlobalResult.Message(ex.Message);
            }
            else if (context.Exception is MySqlException e)
            {
                log.SetMessage(e, context.HttpContext);

                logger.LogCritical(log.APIMessage, log.SourceID, log.SourceController, log.SourceAction, log.SourceRoute, log.TriggerTime, log.ExceptionInfo);
                context.Result = (ObjectResult)GlobalResult.Message(log.ExceptionFeedback);
            }
            else if (context.Exception is APIRegexException et)
            {
                logger.LogWarning("[ {0} ]: {1}", et.GetType().Name, et.Message);
                context.Result = (ObjectResult)GlobalResult.Message(et.Message);
            }

            else
            {
                //log.SetMessage()
                logger.LogError("Other Exception {0} : {1}", context.Exception.GetType().Name, context.Exception.Message);
                context.Result = (ObjectResult)GlobalResult.Message("未知错误，稍后再试");
            }
            context.ExceptionHandled = true;
            await Task.CompletedTask;
        }
    }
}
