using MySql.Data.MySqlClient;
using System.Security.Claims;
using TaskManangerSystem.Actions;

namespace TaskManangerSystem.Services
{

    public class BaseExcption<TSource>() : APILog
    {
        public override string? SourceID { set; get; }
        public override string? SourceController { set; get; }
        public override string? SourceAction { set; get; }
        public override string? SourceRoute { set; get; }

        public virtual string ExceptionFeedback { set; get; } = "WEB API 返回信息";
        public virtual string ExceptionInfo { set; get; } = "控制台信息-错误";

        public override string APIMessage => "\tSorceID > {0};\n\tContrlller > {1};\n\tAction > {2};\n\tRoute > {3};\n\tTime > {4};\n\tInfo > {5};";

    }
    public class DBLinkExcption() : BaseExcption<MySqlException>
    {
        public void SetMessage(MySqlException ex, HttpContext http)
        {
            SourceController = ex.Source;
            SourceAction = "db link";
            SourceID = http.User.Claims.GetClaim(ClaimTypes.Authentication.ToString())?.Value.ToString();
            SourceRoute = SourceRoute = $"{http.Request.Method} {http.Request.Scheme} {http.Request.Host} {http.Request.Path}";

            ExceptionFeedback = "连接失败，稍后再试";
            ExceptionInfo = "数据库未连接";
        }
    }
}
