using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

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

    // public class DBInfo(int count = 1, int pageSize = 120)
    // {
    //     public int Page => (int)Math.Ceiling((double)count / pageSize);// 总页数
    // }
    // public class CompleteInformation<T>(T values, int currentPage = 1) : DBInfo
    // {
    //     // public int Page => (int)Math.Ceiling((double)count / pageSize);// 总页数

    //     public int CurrentPage = currentPage;

    //     public T? Values = values;
    // }

    public static class GlobalResult
    {
        public static ObjectResult Message(string ms)=> new Result<string>(ms).ToObjectResult();
        public static ObjectResult InvalidParameter = new Result<string>("仅允许你自己的信息").ToObjectResult();//无效参数

        public static ObjectResult NoData = new Result<string>("信息不存在").ToObjectResult();
        public static ObjectResult Repetition(string name) => new Result<string>(name + "重复").ToObjectResult();

        public static ObjectResult Cancelled = new Result<string>("你的账户不存在，请重新登录").ToObjectResult();

        public static ObjectResult NoAccess = new Result<string>("访问无效").ToObjectResult();
        public static Result<string> NotAccess = new Result<string>("访问无效");

        public static Result<string> LimitedAuthority = new Result<string>("权限不足");
        
        public static ObjectResult LimitAuth = new Result<string>("权限不足").ToObjectResult();

        public static ObjectResult PWDError = new Result<string>("旧密码错误").ToObjectResult();
        public static ObjectResult NotAdmin = new Result<string>("禁止修改管理员账户").ToObjectResult();
        public static ObjectResult Forbidden = new Result<string>("验证失败").ToObjectResult();
    }
}
