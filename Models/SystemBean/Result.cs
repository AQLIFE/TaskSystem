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
        public static ObjectResult InvalidParameter = new Result<string>(false, "请求拒绝").ToObjectResult();//无效参数

        public static ObjectResult NoData = new Result<string>(false,"数据不存在").ToObjectResult();

        public static ObjectResult NoAccess = new Result<string>(false, "访问无效").ToObjectResult();
        public static Result<string> NotAccess = new Result<string>(false, "访问无效");

        public static Result<string> LimitedAuthority = new Result<string>(false,"权限不足");
        public static ObjectResult LimitAuth = new Result<string>(false,"权限不足").ToObjectResult();

    }

}
