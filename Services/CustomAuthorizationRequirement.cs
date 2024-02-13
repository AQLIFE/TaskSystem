using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TaskManangerSystem.Services.Filters
{
    // 自定义授权要求类
    public class ValidIdRequirement : IAuthorizationRequirement
    {
        // 这个类可以是空的，因为不需要传递任何参数或数据
    }

    // 自定义授权处理程序类
    public class ValidIdHandler : AuthorizationHandler<ValidIdRequirement>
    {
        // 重写 HandleRequirementAsync 方法
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidIdRequirement requirement)
        {
            // 获取用户的声明集合
            var claims = context.User.Claims;
            // 调用自定义的方法来获取 id
            string? id = GetIdByClaims(claims);
            // 如果 id 为空或不存在
            if (string.IsNullOrEmpty(id))
            {
                // 拒绝访问
                context.Fail();
            }
            else
            {
                // 通过授权
                context.Succeed(requirement);
            }
            // 返回完成的任务
            return Task.CompletedTask;
        }

        // 自定义的方法，根据用户的声明获取 id

        private string? GetIdByClaims(IEnumerable<Claim> claims)
            =>(from item in claims where item.ToString() == ClaimTypes.Authentication + ": " + item.Value select item.Value).FirstOrDefault();
    }

    // 在 Startup.cs 文件中，注册自定义 
}