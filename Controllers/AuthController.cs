using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Services;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ManagementSystemContext _context;
        private JsonWebTokenInfo Jwt;

        public AuthController(ManagementSystemContext context, IConfiguration configuration)
        {
            _context = context;
            Jwt = new(configuration);
        }

        [HttpPost]
        public ActionResult<string?> AuthLogin(Part account)
        {
            return EmployeeAccountExists(account) ? Jwt.CreateToken(GetEncryptAccount(account)) : null;
        }

        private bool EmployeeAccountExists(Part account)
        => _context.encrypts.Any(e => e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd);
        private string FindEncryptAccountId(Part account)
        => _context.encrypts.Where(e => e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd).ToList().First().EncryptionId;

        private EncryptAccount GetEncryptAccount(Part account)
        => _context.encrypts.Where(e => e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd).ToList().First();
    }

}
