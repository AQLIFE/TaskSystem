namespace TaskManangerSystem.IServices.BeanServices
{

    public interface IAlias
    {
        public string EmployeeAlias { get; set; }
    }

    public class EAlias : IAlias
    {
        public string EmployeeAlias { get; set; } = String.Empty;
    }
    public interface IPart : IAlias
    {
        // public string EmployeeAlias { get; set; }
        public string EmployeePwd { get; set; }
    }
    public interface IPartInfo : IAlias
    {
        // public string EmployeeAlias { get; set; }
        public int AccountPermission { get; set; }
    }

    public interface IEmployee : IPart, IPartInfo
    {
        public Guid EmployeeId { get; set; }
    }

    public interface IEmployeeConverter
    {
        public IEmployee ToEmployee();
    }

    public interface IEmployeeConverter<T> where T : IPartInfo
    {
        public IEmployee ToEmployee(string pwd, Guid id);
    }

    [Obsolete("该接口将移除，使用IEmployee")]
    public interface IEncrypt : IEmployee
    {
        public string EncryptionId { get; set; }
    }



    public interface ICategoryInfo
    {
        public int SortSerial { set; get; }
        // public int ParentSortSerial { set; get; }// 子类自定义

        public string CategoryName { get; set; }

        public int CategoryLevel { get; set; }

        public string? Remark { get; set; }
    }


    public interface ICategory : ICategoryInfo
    {
        public Guid CategoryId { get; set; }
        public Guid? ParentCategoryId { get; set; }

        // public int SortSerial { set; get; }

        // public string CategoryName { get; set; }

        // public int CategoryLevel { get; set; }

        // public string? Remark { get; set; }
    }

    public interface ICustomerInfo
    {
        public string CustomerName { get; set; }
        public string? CustomerContactWay { get; set; }
        public string? CustomerAddress { get; set; }
    }


    public interface ICustomer : ICustomerInfo
    {
        public Guid CustomerId { get; set; }
        // public string CustomerName { get; set; }
        // public string? CustomerContactWay { get; set; }
        // public string? CustomerAddress { get; set; }
        public Guid CustomerType { get; set; }
        public int ClientGrade { get; set; }
        public DateTime AddTime { get; set; }
    }
}