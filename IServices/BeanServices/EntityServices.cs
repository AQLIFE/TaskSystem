
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
    }

    public interface IEncrypt : IEmployee
    {
        public string EncryptionId { get; set; }
    }


    public interface ICategory{
        public Guid CategoryId { get; set; }
        public Guid? ParentCategoryId { get; set; }

        public int SortSerial{set;get;}

        public string CategoryName { get; set; }

        public int CategoryLevel { get; set; }

        public string Remark { get; set; }
    }

    public interface ICateInfo{
        public int SortSerial{set;get;}
        public int ParentSortSerial{set;get;}

        public string CategoryName { get; set; }

        public int CategoryLevel { get; set; }

        public string Remark { get; set; }
    }
}