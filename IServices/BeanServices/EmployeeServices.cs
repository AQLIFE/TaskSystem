
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.IServices.BeanServices
{
    public interface IPart
    {
        public string EmployeeAlias { get; set; }
        public string EmployeePwd { get; set; }
    }
    public interface IPartInfo
    {
        public string EmployeeAlias { get; set; }
        public int AccountPermission { get; set; }
    }

    public interface IEmployee : IPart,IPartInfo
    {
        public Guid EmployeeId { get; set; }
        // public AliasAccount ToAliasAccount(bool ss = false, char cr = '*');
    }

    public interface IEncrypt : IEmployee
    {
        public string EncryptionId { get; set; }
    }
}