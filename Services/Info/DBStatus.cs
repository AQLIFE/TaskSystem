using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Tool;

namespace TaskManangerSystem.Models.SystemBean
{
    public class DBStatus(ManagementSystemContext context)
    {
        public bool EmployeesStatus { get; set; } = context.employees.ToList().IsNullOrEmpty();
        public bool CatrgoryStatus { get; set; } = context.categories.ToList().IsNullOrEmpty();
        public bool TaskCustomerStatus { get; set; } = context.customers.ToList().IsNullOrEmpty();


        public void Update()
        {
            EmployeesStatus = context.employees.ToList().IsNullOrEmpty();
            CatrgoryStatus = context.categories.ToList().IsNullOrEmpty();
            TaskCustomerStatus = context.customers.ToList().IsNullOrEmpty();
        }
    }
}