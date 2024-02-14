using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Models.SystemBean
{
    /// <summary>
    /// 用于返回脱敏数据
    /// </summary>
    public class AliasAccount : BaseAlias,IAlias
    {
        public AliasAccount() { }

        public EmployeeAccount ToEmployeeAccount(Guid? id = null)
            => new (this, id ?? Guid.NewGuid());
        
        public AliasAccount(IEmployee employee, bool ss = false, char cr = '*')
        {
            this.EmployeeAlias = employee.EmployeeAlias;
            this.EmployeePwd = !ss ? new( cr, 10) : employee.EmployeePwd;
            this.AccountPermission = employee.AccountPermission;
        }

    }
}