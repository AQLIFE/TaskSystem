using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Services;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.SystemBean;
using Microsoft.AspNetCore.Authorization;

namespace TaskManangerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController,AllowAnonymous]
    public class AuthController(ManagementSystemContext context) : ControllerBase
    {
        private EmployeeActions employee = new EmployeeActions(context);
        
        [HttpPost]
        public ActionResult<string?> AuthLogin(Part account)
                => employee.LoginCheck(account) ?
                new BearerInfo(employee.GetEmployeeByName(account.EmployeeAlias)!).CreateToken() :
                "登录校验不通过";
    }

}
