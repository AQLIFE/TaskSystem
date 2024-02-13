using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Services;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ManagementSystemContext context) : ControllerBase
    {
        [HttpPost]
        public ActionResult<string?> AuthLogin(Part account)
        {
            return EmployeeAccountExists(account) ? new JsonWebTokenInfo().CreateToken(GetEncryptAccount(account)) : null;
        }

        private bool EmployeeAccountExists(Part account)
        => context.encrypts.Any(e => e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd);
        private string FindEncryptAccountId(Part account)
        => context.encrypts.Where(e => e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd).ToList().First().EncryptionId;

        private EncryptAccount GetEncryptAccount(Part account)
        => context.encrypts.Where(e => e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd).ToList().First();
    }

}
