using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Controllers
{
    // [ApiController, Authorize, Route("api/[controller]")]
    public class DBInfoController(ManagementSystemContext systemContext) : ControllerBase
    {
        public DBInfo GetInfoByEmployee() => new(systemContext.employees.Count());
        public DBInfo GetInfoByCategory() => new(systemContext.categories.Count());

        // public DBInfo GetInfoByEmployee() => new(systemContext.employees.Count());
        // public DBInfo GetInfoByEmployee() => new(systemContext.employees.Count());
        // public DBInfo GetInfoByEmployee() => new(systemContext.employees.Count());
    }
}