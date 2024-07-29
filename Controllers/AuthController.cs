using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private EmployeeRepositoryAsync ERA => new(context);




        //[HttpGet]//获取自己的ID
        private string HashId => HttpContext.Items["HashId"]!.ToString()!;



        [HttpGet("info")]//获取自己的信息
        public async Task<EmployeeAccountForSelectOrUpdate?> GetEmployeeSystemAccount(string hashId = "")
        => mapper.Map<EmployeeAccountForSelectOrUpdate>(await ERA.TryGetEmployeeByHashIdAsync(hashId == "" ? HashId : hashId));



        [HttpGet("list"), Authorize(Policy = "admin")]
        public async Task<PageContext<EmployeeAccountForSelectOrUpdate>?> GetAuthList([FromHeader] bool isUp = true, int page = 1, int pageContext = SystemInfo.PageSize)
        => mapper.Map<PageContext<EmployeeAccountForSelectOrUpdate>>(await ERA.SearchAsync(page, pageContext, isUp));





        [HttpPost("login"), AllowAnonymous]//登录
        public async Task<string?> PostLogin(EmployeeAccountForLoginOrAdd account)
                => await ERA.LoginCheckAsync(account) ?
                new BearerInfo((await ERA.TryGetEmployeeByNameAsync(account.EmployeeAlias))!).CreateToken() :
                "登录校验不通过或者账号不存在";




        [HttpPost("register"), AllowAnonymous]//增加
        public async Task<bool> PostRegister(EmployeeAccountForLoginOrAdd info)
            => await ERA.ExistsEmployeeByNameAsync(info.EmployeeAlias) ? false :
             await ERA.AddAsync(mapper.Map<EmployeeAccount>(info));






        [HttpPut("uplevel")]
        public async Task<bool> Uplevel(string hashId = "")
        {
            EmployeeAccount? employeeAccount = await ERA.TryGetEmployeeByHashIdAsync(hashId == "" ? HashId : hashId);
            return employeeAccount is EmployeeAccount ? await ERA.UpdateLevelAsync(employeeAccount) : false;
        }

        [HttpPut("secret")]//修改密码
        public async Task<bool> PutSecret(string pwd, string oldPwd, string hashId = "")
        {
            EmployeeAccount? employeeAccount = await ERA.TryGetEmployeeByHashIdAsync(hashId == "" ? HashId : hashId);
            return employeeAccount is EmployeeAccount x ? await ERA.UpdatePwdAsync(x, pwd, oldPwd) : false;
        }



        [HttpDelete("delete")]//注销账户(封存)
        public async Task<bool> DeleteAccount(string hashId = "")
        {
            var a = await ERA.TryGetEmployeeByHashIdAsync(hashId == "" ? HashId : hashId);

            return a is not null ? await ERA.DisabledAsync(a) : false;
        }
    }

}
