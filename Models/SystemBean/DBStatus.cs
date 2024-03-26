using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Models.SystemBean{
    public class DBStatus{
        public bool EmployeesStatus{get;set;}
        public bool CatrgoryStatus{get;set;}
        public bool TaskCustomerStatus{get;set;}

        public DBStatus(){}
        public DBStatus(bool employeesStatus,bool catrgoryStatus,bool taskCustomerStatus){
            EmployeesStatus = employeesStatus;
            CatrgoryStatus = catrgoryStatus;
            TaskCustomerStatus = taskCustomerStatus;
        }

        public DBStatus(ManagementSystemContext context){
            EmployeesStatus = context.employees.ToList().IsNullOrEmpty();
            CatrgoryStatus = context.categories.ToList().IsNullOrEmpty();
            TaskCustomerStatus = context.customers.ToList().IsNullOrEmpty();
        }
    }
}