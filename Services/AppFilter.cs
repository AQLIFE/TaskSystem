using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{

    public class Log<T, U>
    {
        public bool status { set; get; }

        public string name { set; get; }

        public string funcName { set; get; }

        public T invalidParameterValue { set; get; }
        public U parameterValue { set; get; }

        public string VisualLogInformation{get
        {
            return $"访问状态：{(status?"succeed":"fail")},请求用户:{name??"未知用户"},请求方法:{funcName},请求参数：{invalidParameterValue},还原参数：{parameterValue}";
        }}
    }
    public class AppFilter : IActionFilter
    {

        private ManagementSystemContext _context;
        private ILoggerFactory _logger;

        public AppFilter(ManagementSystemContext context)
        {
            _context = context;
            // _logger = 
        }

        public void OnActionExecuting(ActionExecutingContext action)
        {
            // if (action.HttpContext.User.Claims != null)
            // {
            //     string? id = (from item in action.HttpContext.User.Claims where item.ToString() == ClaimTypes.Authentication + ": " + item.Value select item.Value).FirstOrDefault();
            //     //拿到ID
            //     // Console.WriteLine($"id,{id}");
            //     if (id != null)
            //     {
            //         EncryptAccount? obj = _context.encrypts.Where(e => e.EncryptionId == id).FirstOrDefault();
            //         if (obj != null)//存在该用户
            //         {
            //             if (action.ActionArguments.ContainsKey("id") && action.ActionArguments["id"]?.ToString() != obj?.EncryptionId && obj?.AccountPermission < 90)
            //             {
            //                 Console.WriteLine($"已拦截越权操作 用户：{obj?.EmployeeAlias} 越权参数:{action.ActionArguments["id"]},还原参数:{obj?.EncryptionId}");
            //                 action.ActionArguments["id"] = obj?.EncryptionId;
            //             }

            //             if (action.ActionArguments.ContainsKey("employeeSystemAccount"))
            //             {
            //                 action.ActionArguments["employeeSystemAccount"] = obj?.ToAliasAccount();
            //             }
            //             Console.WriteLine($"合法访问,{obj?.EmployeeAlias},{action.ActionDescriptor.DisplayName}");
            //         }
            //     }
            //     else Console.WriteLine($"异常访问,未知用户,{action.ActionDescriptor.DisplayName}");
            // }

            if (action.HttpContext.User.Claims.IsNullOrEmpty())
            {
                string? id = (from item in action.HttpContext.User.Claims where item.ToString() == ClaimTypes.Authentication + ": " + item.Value select item.Value).FirstOrDefault();
                if (id.IsNullOrEmpty())
                {
                    EncryptAccount? obj = _context.encrypts.Where(e => e.EncryptionId == id).FirstOrDefault();
                    if (obj != null)
                        if (action.ActionArguments.ContainsKey("id") && action.ActionArguments["id"]?.ToString() != obj?.EncryptionId && obj?.AccountPermission < 90)
                        {
                            // _logger.LogInformation()
                            action.ActionArguments["id"] = obj?.EncryptionId;
                        }
                    if (action.ActionArguments.ContainsKey("employeeSystemAccount"))
                    {
                        action.ActionArguments["employeeSystemAccount"] = obj?.ToAliasAccount();
                    }
                    // Console.WriteLine($"合法访问,{obj?.EmployeeAlias},{action.ActionDescriptor.DisplayName}");
                }
                else action.Result = new Result<string>(false,"请重新登录").ToObjectResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext action)
        {
            HttpRequest obj = action.HttpContext.Request;
            ObjectResult item = action.Result as ObjectResult ?? throw new Exception("没有这个类型");
            action.Result = new Result<Object?>(!(action.Exception != null || item.Value == null), item.Value).ToObjectResult();
            // if(action.Exception!=null || item.Value==null ){
            //     action.Result = new ObjectResult(new Result<string>(false,"请求失败"));
            // }
            // else {
            //     action.Result =  new ObjectResult(new Result<object>(true,item.Value));
            // }
        }
    }
}