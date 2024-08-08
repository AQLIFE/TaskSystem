using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services.Repository;
using TaskManangerSystem.Services.Tool;

namespace TaskManangerSystem.Controllers
{
    [ApiController, Route("api/[controller]"), AllowAnonymous]
    public class SystemController(ManagementSystemContext context) : ControllerBase
    {
        private DBAction action = new(context);
        private DBStatus status = new(context);


        [HttpGet("status")]
        public DBStatus ExistsDb() => status;



        [HttpPost("init")]
        public async Task<int> InitDb() => status switch
        {
            DBStatus s when s.EmployeesStatus => await action.InitAdminAccount(),
            DBStatus s when s.CatrgoryStatus => await action.InitCategory(),
            DBStatus s when s.TaskCustomerStatus => await action.InitCustomer(),
            _ => 0
        };
    }


    [ApiController, Route("api/{HashRoute}"), AllowAnonymous]
    public class EndpointController() : ControllerBase
    {
        private ErrorActions errorActions = new();

        [HttpGet("{X}")] public async Task<string?> MatchGet() => await errorActions.Get();
        [HttpPost("{X}")] public async Task<bool> MatchPost() => await errorActions.Post();
        [HttpPatch("{X}")] public async Task<string?> MatchPatch() => await errorActions.Fetch();
        [HttpDelete("{X}")] public async Task<bool> MatchDelete() => await errorActions.Delete();
        [HttpPut("{X}")] public async Task<bool> MatchPut() => await errorActions.Put();

    }
}