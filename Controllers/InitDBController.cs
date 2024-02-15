using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Services;

namespace InitDb
{
    [ApiController, Route("api/[controller]")]
    public class InitDBController(ManagementSystemContext context) : ControllerBase
    {
        [HttpGet("status")]
        public bool ExitsEmplyee() => context.employees.ToList().IsNullOrEmpty();

        [HttpGet("execute")]
        public int InitEmplyee()
        {
            if (ExitsEmplyee())
                return AddAdminAccount();
            return 0;
        }

        private int AddAdminAccount()
        {
            var admin = new EmployeeAccount()
            {
                EmployeeId = Guid.NewGuid(),
                EmployeeAlias = "admin",
                EmployeePwd = "admin@123",
                AccountPermission = 99
            };

            context.Entry<EmployeeAccount>(admin).State = EntityState.Added;
            return context.SaveChanges();
        }
        // public bool  Exits()=>context.encrypts.ToList() is null;
        // public bool  ExitsEmplyee()=>context.tasks.ToList() is null;
        // public bool  ExitsEmplyee()=>context.employees.ToList() is null;
    }
}