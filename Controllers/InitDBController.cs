using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Services;

namespace InitDb
{
    [ApiController,Route("api/[controller]")]
    public class InitDBController :ControllerBase
    {
        private readonly ManagementSystemContext _context;
        public InitDBController(ManagementSystemContext context)
        {
            this._context = context;
        }

        [HttpGet("status")]
        public bool ExitsEmplyee() => _context.employees.ToList().IsNullOrEmpty();

        [HttpGet("execute")]
        public int InitEmplyee()
        {
            if (ExitsEmplyee())
                return AddAdminAccount();
            return 0;
        }

        private int AddAdminAccount()
        {
            var admin = new EmployeeAccount(){
                EmployeeId = Guid.NewGuid(),
                EmployeeAlias = "admin",
                EmployeePwd = "admin@123",
                AccountPermission = 99
            };

            _context.Entry<EmployeeAccount>(admin).State = EntityState.Added;
            return _context.SaveChanges();
        }
        // public bool  Exits()=>_context.encrypts.ToList() is null;
        // public bool  ExitsEmplyee()=>_context.tasks.ToList() is null;
        // public bool  ExitsEmplyee()=>_context.employees.ToList() is null;
    }
}