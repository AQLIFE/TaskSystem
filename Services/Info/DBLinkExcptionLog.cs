using MySql.Data.MySqlClient;
using System.Security.Claims;
using TaskManangerSystem.Services.Tool;

namespace TaskManangerSystem.Services.Info
{

    public class BaseExcption<TSource>() : APILog
    {
        public override string? SourceID { set; get; }
        public override string? SourceController { set; get; }
        public override string? SourceAction { set; get; }
        public override string? SourceRoute { set; get; }

        public virtual string ExceptionFeedback { set; get; } = ErrorMessage.FeedBack;
        public virtual string ExceptionInfo { set; get; } = ErrorMessage.ConsoleExceptionInfo;

        public override string APIMessage => "\tSorceID > {0};\n\tContrlller > {1};\n\tAction > {2};\n\tRoute > {3};\n\tTime > {4};\n\tInfo > {5};";

    }
    public class DBLinkExcptionLog() : BaseExcption<MySqlException>
    {
        public void SetMessage(MySqlException ex, HttpContext http)
        {
            SourceController = ex.Source;
            SourceAction = "db link";
            SourceID = http.User.Claims.GetClaim(ClaimTypes.Authentication.ToString())?.Value.ToString();
            SourceRoute = SourceRoute = $"{http.Request.Method} {http.Request.Scheme} {http.Request.Host} {http.Request.Path}";

            ExceptionFeedback =base.ExceptionFeedback;
            ExceptionInfo =ErrorMessage.DBUnLink;
        }
    }
}
