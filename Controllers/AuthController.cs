using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Services;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ManagementSystemContext context) : ControllerBase
    {
        private EmployeeActions employee = new EmployeeActions(context);
        
        [HttpPost]
        public ActionResult<string?> AuthLogin(Part account)
            => employee.LoginCheck(account) ?
             new BearerInfo().CreateToken(employee.GetEmployeeByName(account.EmployeeAlias)!) :
              "登录校验不通过";
    }

}
