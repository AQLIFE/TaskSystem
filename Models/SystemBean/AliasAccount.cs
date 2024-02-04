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

    // public class EmployeeAccount : BaseEmployee
    // {
    //     public EmployeeAccount(AliasAccount alias, Guid id)
    //     {
    //         this.EmployeeId = id;
    //         this.EmployeeAlias = alias.EmployeeAlias;
    //         this.EmployeePwd = alias.EmployeePwd;
    //         this.AccountPermission = alias.AccountPermission;
    //     }
    // }

    // public class AliasConverter <T>  where T :IEmployee
    // {
    //     // private T _obj;

    //     public static AliasEmployeeSystemAccount Result;

    //     public AliasConverter(T value)
    //     {
    //         // _obj = value;
    //         Result = ConvertToAlias(value);
    //     }

    //     private AliasEmployeeSystemAccount ConvertToAlias(IAlias alias,bool status=false,char cr='*')
    //     {
    //         switch (alias)
    //         {
    //             case EncryptEmployeeSystemAccount encrypt:
    //                 return new (encrypt,statsus,cr);
    //             case EmployeeSystemAccount account:
    //                 return new (account,statsus,cr);
    //             default:
    //                 throw new ArgumentException("Unsupported type for conversion.");
    //         }
    //     }
    // }
}