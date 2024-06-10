using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class AuthController(ManagementSystemContext context, IMapper mapper) : ControllerBase
    {
        private EmployeeActions employeeAction = new EmployeeActions(context, mapper);




        [HttpGet]//获取自己的ID
        public string? GetHashId() => HttpContext.Items["HashId"]?.ToString();



        [HttpGet("info")]//获取自己的信息
        public async Task<EmployeeAccountForSelectOrUpdate?> GetEmployeeSystemAccount(string hashId)
        // => mapper.Map<EmployeeAccountForSelectOrUpdate>( await employeeAction.GetEmployeeByHashId( HttpContext.Items["HashId"]?.ToString()??Guid.Empty.ToString() ));
        => mapper.Map<EmployeeAccountForSelectOrUpdate>(await employeeAction.GetEmployeeByHashIdAsync(hashId));



        [HttpGet("list"), Authorize(Policy = "admin")]
        public async Task<PageContext<EmployeeAccountForSelectOrUpdate>?> GetAuthList([FromHeader]bool isUp=true,int page = 1, int pageContext = 100)
        // =>mapper.Map<List<IPartInfo>>(await employeeAction.SearchFor(page, pageSize, e => e.EmployeeAlias));
        => await employeeAction.SearchAsync(page, pageContext,isUp);





        [HttpPost("login"), AllowAnonymous]//登录
        public async Task<string?> PostLogin(EmployeeAccountForLoginOrAdd account)
                => await employeeAction.LoginCheckAsync(account) ?
                new BearerInfo((await employeeAction.GetEmployeeByNameAsync(account.EmployeeAlias))!).CreateToken() :
                "登录校验不通过或者账号不存在";




        [HttpPost("register"), AllowAnonymous]//增加
        public async Task<bool> PostRegister(EmployeeAccountForLoginOrAdd info)
            => await employeeAction.ExistsEmployeeByNameAsync(info.EmployeeAlias) ? false :
             await employeeAction.AddInfoAsync(mapper.Map<EmployeeAccount>(info));






        [HttpPut("uplevel")]
        public async Task<bool> Uplevel(string hashId)
        {
            EmployeeAccount? employeeAccount = await employeeAction.GetEmployeeByHashIdAsync(hashId);
            return employeeAccount is EmployeeAccount ? await employeeAction.UpdateLevelAsync(employeeAccount) : false;
        }

        [HttpPut("secret")]//修改密码
        public async Task<bool> PutSecret(string hashId, string pwd, string oldPwd)
        {
            EmployeeAccount? employeeAccount = await employeeAction.GetEmployeeByHashIdAsync(hashId);
            return employeeAccount is EmployeeAccount x ? await employeeAction.UpdatePwdAsync(x, pwd, oldPwd) : false;
        }



        [HttpDelete("delete")]//注销账户(封存)
        public async Task<bool> DeleteAccount(string hashId)
        {
            var a = await employeeAction.GetEmployeeByHashIdAsync(hashId);

            return a is not null ? await employeeAction.DisabledAsync(a) : false;
        }
    }

}
