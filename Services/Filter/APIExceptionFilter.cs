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
                logger.LogWarning($"[ {ErrorMessage.FailData} ] : {x}");
                context.Result = (ObjectResult)GlobalResult.Message(ErrorMessage.FailData);
            }            
            else if (context.Exception is MySqlException e)
            {
                log.SetMessage(e, context.HttpContext);

                logger.LogCritical(log.APIMessage, log.SourceID, log.SourceController, log.SourceAction, log.SourceRoute, log.TriggerTime, log.ExceptionInfo);
                context.Result = (ObjectResult)GlobalResult.Message(log.ExceptionFeedback);
            }           
            else if (context.Exception is EntityException entity)
            {
                logger.LogWarning(entity.ShowException);
                context.Result = (ObjectResult)GlobalResult.Message(entity.Message);
            }
            else
            {
                logger.LogError($"Other Exception {context.Exception.GetType().Name} : {context.Exception.Message}");
                context.Result = (ObjectResult)GlobalResult.Message(ErrorMessage.UnknowError);
            }
            context.ExceptionHandled = true;
            await Task.CompletedTask;
        }
    }
}
