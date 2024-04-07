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
    public class AuthController(ManagementSystemContext context) : ControllerBase
    {
        private EmployeeActions employeeAction = new EmployeeActions(context);

        [HttpGet]
        public string? GetHashId() =>
            HttpContext.Items["HashId"]?.ToString();


        [HttpPost, AllowAnonymous]
        public ActionResult<string?> AuthLogin(Part account)
                => employeeAction.LoginCheck(account) ?
                new BearerInfo(employeeAction.GetEmployeeByName(account.EmployeeAlias)!).CreateToken() :
                "登录校验不通过或者账号不存在";


        [HttpPut("pwd")]
        public async Task<string?> AuthPwd(string id, string pwd, string oldPwd)
        {
            EmployeeAccount? employeeAccount = employeeAction.GetEmployee(id);
            if (employeeAccount == null || oldPwd == pwd || oldPwd != employeeAccount.EmployeePwd) return null;
            else
            {
                employeeAccount.EmployeePwd = pwd;
                context.Entry<EmployeeAccount>(employeeAccount).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return employeeAccount.EmployeeAlias;
            }
        }
    }


    [ApiController, Route("api/[controller]"), AllowAnonymous]
    public class InitDBController(ManagementSystemContext context) : ControllerBase
    {
        private DBAction action = new(context);
        private DBStatus? status;
        [HttpGet("status")]
        public DBStatus ExistsDb() => status = new(context);

        [HttpGet("execute")]
        public int InitEmplyee() => ExistsDb() switch
        {
            DBStatus s when s.EmployeesStatus => action.AddAdminAccount(),
            DBStatus s when s.CatrgoryStatus => action.AddCategory(),
            DBStatus s when s.TaskCustomerStatus => action.AddCustomer(),
            _ => 0
        };
    }

}
