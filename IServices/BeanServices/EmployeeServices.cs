using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.IServices.BeanServices
{
    public interface IAuth
    {
        public string EncryptionId { get; set; }
        public string EmployeeAlias { get; set; }
        public int AccountPermission { get; set; }
    }

    public interface IPart
    {
        public string EmployeeAlias { get; set; }
        public string EmployeePwd { get; set; }
    }

    public class Part
    {
        public string EmployeeAlias { get; set; }
        public string EmployeePwd { get; set; }


        public EmployeeAccount ToEmployeeAccount(Guid? id = null)
        => new(this, id ?? Guid.NewGuid());
    }
    public interface IAlias : IPart
    {
        // public string EmployeeAlias { get; set; }
        // public string EmployeePwd { get; set; }
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
            => new(this, ss == default ? false : true, cr == default ? '*' : cr);
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