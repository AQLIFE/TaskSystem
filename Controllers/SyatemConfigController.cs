using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Services;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.SystemBean;
using Microsoft.AspNetCore.Authorization;

namespace TaskManangerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController, AllowAnonymous]
    public class AuthController(ManagementSystemContext context) : ControllerBase
    {
        private EmployeeActions employee = new EmployeeActions(context);

        [HttpPost]
        public ActionResult<string?> AuthLogin(Part account)
                => employee.LoginCheck(account) ?
                new BearerInfo(employee.GetEmployeeByName(account.EmployeeAlias)!).CreateToken() :
                "登录校验不通过";
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
