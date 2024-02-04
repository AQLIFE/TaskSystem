using TaskManangerSystem.Models;
using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.DbContextConfg;

namespace TaskManangerSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController :ControllerBase
    {
        private readonly ManagementSystemContext _context;
        
        public AuthController(ManagementSystemContext context)
        {
            _context = context;
        }

        // [HttpGet]
        // public async Task<IActionResult> AuthLogin(AliasEmployeeSystemAccount account){
        //     if(_context.AliasMds.Find(x => x. ) )
        // }
    }

}
