using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace TaskManangerSystem.Models.SystemBean
{

    public class Result<T>(T? data, bool status = false)
    {
        [JsonInclude]
        public bool Status = status;
        [JsonInclude]
        public T? Data = data;

        public ObjectResult ToObjectResult() => new ObjectResult(this);
    }

    public static class GlobalResult
    {
        public static ObjectResult Message(string ms) => new Result<string>(ms).ToObjectResult();
        public static ObjectResult InvalidParameter = new Result<string>("仅允许你自己的信息").ToObjectResult();//无效参数

        public static ObjectResult NoData = new Result<string>("信息不合法或不存在").ToObjectResult();
        // public static ObjectResult Data = new Result<string>("信息重复").ToObjectResult();
        public static ObjectResult Repetition(string name) => new Result<string>(name + "重复").ToObjectResult();

        public static ObjectResult Cancelled = new Result<string>("你的账户不存在，请重新登录").ToObjectResult();

        public static ObjectResult NoAccess = new Result<string>("访问无效").ToObjectResult();

        public static Result<string> LimitedAuthority = new Result<string>("权限不足");

        public static ObjectResult LimitAuth = new Result<string>("权限不足").ToObjectResult();

        public static ObjectResult PWDError = new Result<string>("旧密码错误").ToObjectResult();
        public static ObjectResult NotAdmin = new Result<string>("禁止修改管理员账户").ToObjectResult();
        public static Result<string> Forbidden = new Result<string>("验证失败");
        public static Result<string> NotAccess = new Result<string>("访问无效");

    }
}
