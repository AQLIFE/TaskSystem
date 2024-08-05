using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TaskManangerSystem.Actions;

namespace TaskManangerSystem.Services
{
    public class APILog()
    {
        public virtual string? SourceID { set; get; }
        public virtual string? SourceController { set; get; }
        public virtual string? SourceAction { set; get; }
        public virtual string? SourceRoute { set; get; }
        public virtual DateTime TriggerTime { set; get; } = DateTime.UtcNow;


        public void SetMessage(ActionExecutingContext requesetActionContext)
        {
            var http = requesetActionContext.HttpContext;

            SourceController = requesetActionContext.Controller.GetType().FullName;
            SourceID = http.User.Claims.GetClaim(ClaimTypes.Authentication.ToString())?.Value.ToString();
            SourceAction = requesetActionContext.ActionDescriptor.DisplayName;
            SourceRoute = $"{http.Request.Method} {http.Request.Scheme} {http.Request.Host} {http.Request.Path}";
        }

        public void SetMessage(AuthorizationHandlerContext context)
        {
            SourceID = context.User.Claims.GetClaim(ClaimTypes.Authentication)?.Value;
            SourceController = context.GetType().Name;
        }


        public virtual string APIMessage => "SorceID > {0};\n\tContrlller > {1};\n\tAction > {2};\n\tRoute > {3};\nTime > {4};";

    }



}