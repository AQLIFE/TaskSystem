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

        public static implicit operator ObjectResult(Result<T> result) => new(result);
    }

    public static class GlobalResult
    {
        public static readonly Result<string> Cancelled = new("你的账户不存在，请重新登录");
        public static readonly Result<string> PWDError = new("旧密码错误");
        public static readonly Result<string> NotAdmin = new("禁止修改管理员账户");
        public static readonly Result<string> InvalidParameter = new("无效数据");
        public static readonly Result<string> Forbidden = new("验证失败");
        public static readonly Result<string> NotAccess = new("无权访问");

        public static Result<string> Message(string ms) => new(ms);
        public static Result<string> Repetition(string name) => new(name + "重复");
        public static Result<object?> Succeed(object? obj) => new(obj, obj is not null);

    }
}
