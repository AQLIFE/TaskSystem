using TaskManangerSystem.Models;

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
    }


    public interface IEmployee : IAlias
    {
        public Guid EmployeeId { get; set; }
        // public string EmployeeAlias { get; set; }
        // public string EmployeePwd { get; set; }
        // public int AccountPermission { get; set; }
    }
    public abstract class BaseEmployee : BaseAlias, IEmployee
    {
        public Guid EmployeeId { get; set; }
        // public string EmployeeAlias { get; set; }
        // public string EmployeePwd { get; set; }
        // public int AccountPermission { get; set; }
    }

    public interface IEncrypt :IEmployee
    {
        public string EncryptionId{get;set;}
    }
}