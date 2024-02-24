using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace InitDb
{
    [ApiController, Route("api/[controller]")]
    public class InitDBController(ManagementSystemContext context) : ControllerBase
    {
        private DBAction action = new(context);
        private DBStatus status;
        [HttpGet("status")]
        public DBStatus ExistsDb() => status = new(context);

        [HttpGet("execute")]
        public int InitEmplyee() => ExistsDb() switch
        {
            DBStatus s when s.EmployeesStatus    => action.AddAdminAccount(),
            DBStatus s when s.CatrgoryStatus     => action.AddCategory(),
            DBStatus s when s.TaskCustomerStatus => action.AddCustomer(),
            _ => 0
        };
    }
}