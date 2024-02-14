using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManangerSystem.Models.SystemBean
{

    public class Result<T>
    {
        [JsonInclude]
        public  bool status = false;
        [JsonInclude]
        public  T? data;

        public Result(){}

        public Result(bool bl,T obj){
            this.status = bl;
            this.data = obj;
        }

        public ObjectResult ToObjectResult(){
            return new ObjectResult(this);
        }
    }

    public static class GlobalResult{
        public static ObjectResult InvalidParameter = new Result<string>(false, "仅允许你自己的信息").ToObjectResult();//无效参数

        public static ObjectResult NoData = new Result<string>(false,"信息不存在").ToObjectResult();
        public static ObjectResult Repetition(string name )=>new Result<string>(false,name+"重复").ToObjectResult(); 

        public static ObjectResult Cancelled = new Result<string>(false,"你的账户不存在，请重新登录").ToObjectResult();

        public static ObjectResult NoAccess = new Result<string>(false, "访问无效").ToObjectResult();
        public static Result<string> NotAccess = new Result<string>(false, "访问无效");

        public static Result<string> LimitedAuthority = new Result<string>(false,"权限不足");
        public static ObjectResult LimitAuth = new Result<string>(false,"权限不足").ToObjectResult();

        public static ObjectResult PWDError = new Result<string>(false,"旧密码错误").ToObjectResult();
    }

}
