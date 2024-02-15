using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Models.SystemBean
{
    public class BasePart(BaseEmployee? employee) : IPart
    {
        public virtual string? EmployeeAlias { get; set; } = employee?.EmployeeAlias;
        public virtual string? EmployeePwd { get; set; } = employee?.EmployeePwd;

        public BasePart():this(null){ }
        public virtual IEmployee ToEmployee() => new BaseEmployee(this);
    }

    public class BasePartInfo(IEmployee? employee) : IPartInfo
    {
        public virtual string EmployeeAlias { get; set; } = ((IPart)employee!).EmployeeAlias;
        public virtual int AccountPermission { get; set; } =employee.AccountPermission;

        // public BasePartInfo(IEmployee employee)
        // {
        //     EmployeeAlias = ((IPart)employee).EmployeeAlias;
        //     AccountPermission = employee.AccountPermission;
        // }
        public BasePartInfo() :this(null){ }
        public virtual IEmployee ToEmployee(string pwd, Guid id) => new BaseEmployee(this, pwd, id);
    }

    public class BaseEmployee : IEmployee
    {
        public BaseEmployee(IPart obj)
        {
            EmployeeId = Guid.NewGuid();
            EmployeeAlias = obj.EmployeeAlias;
            EmployeePwd = obj.EmployeePwd;
            AccountPermission = 1;
        }// 新增用户

        public BaseEmployee(IPartInfo obj, string pwd, Guid id)
        {
            EmployeeId = id;
            EmployeeAlias = obj.EmployeeAlias;
            EmployeePwd = pwd;
            AccountPermission = obj.AccountPermission;
        }// 查找用户

        public virtual Guid EmployeeId { get; set; }
        public virtual string EmployeeAlias { get; set; }
        public virtual string EmployeePwd { get; set; }
        public virtual int AccountPermission { get; set; }
        public virtual IPartInfo ToBasePartInfo() => new BasePartInfo(this);
        public virtual IPart ToBasePart() => new BasePart(this);

        public BaseEmployee() { }

    }

    public class BaseEncrypt : IEncrypt
    {
        public virtual string EncryptionId { get; set; }
        public virtual Guid EmployeeId { get; set; }
        public virtual string EmployeeAlias { get; set; }
        public virtual string EmployeePwd { get; set; }
        public virtual int AccountPermission { get; set; }
        public virtual IPartInfo ToBasePartInfo() => new BasePartInfo(this);

        public BaseEncrypt() { }
    }

}