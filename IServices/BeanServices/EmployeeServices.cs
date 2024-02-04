using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.IServices.BeanServices
{
    public interface IAlias
    {
        public string EmployeeAlias { get; set; }
        public string EmployeePwd { get; set; }
        public int AccountPermission { get; set; }
    }
    public abstract class BaseAlias : IAlias
    {
        public string EmployeeAlias { get; set; }
        public string EmployeePwd { get; set; }
        public int AccountPermission { get; set; }

        //  Error : 不能创建抽象方法，否则会让后继承类也需要实现该方法
        // public abstract IEmployee ToEmployeeSystemAccount(Guid? id=null);
    }


    public interface IEmployee : IAlias
    {
        public Guid EmployeeId { get; set; }
        public AliasAccount ToAliasAccount(bool ss = false, char cr = '*');
    }

    public abstract class BaseEmployee : BaseAlias, IEmployee
    {
        public Guid EmployeeId { get; set; }

        public AliasAccount ToAliasAccount(bool ss = false, char cr = '*')
            => new(this, ss, cr);
    }

    public interface IEncrypt : IEmployee
    {
        public string EncryptionId { get; set; }
    }

    public abstract class BaseEncrypt : BaseEmployee, IEncrypt
    {
        public string EncryptionId { get; set; }
    }
}