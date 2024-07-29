namespace TaskManangerSystem.IServices
{

    public interface IAlias
    {
        public string EmployeeAlias { get; set; }
    }

    public class EAlias : IAlias
    {
        public string EmployeeAlias { get; set; }
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
        public Guid? CustomerType { get; set; }
        public int ClientGrade { get; set; }
        public DateTime AddTime { get; set; }
    }

    public interface IInventory
    {
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal ProductCost { get; set; }

        public string ProductModel { get; set; }

    }

    public interface IInventoryInfo : IInventory
    {
        public Guid ProductId { get; set; }
        public Guid? ProductType { get; set; }
    }


    public interface ITaskAttair
    {
        public string Content { get; set; }
        public decimal Cost { get; set; }
    }

    public interface ITaskAttairInfo : ITaskAttair
    {
        public int Serial { set; get; }
        public DateTime Time { get; set; }
        //public string? Content { get; set; } 
        //public decimal Cost { get; set; } 
    }

    public interface ITaskAttairs : ITaskAttairInfo
    {
        public Guid TaskId { get; set; }
        public Guid? TaskType { set; get; }
        public Guid? CustomerId { get; set; }
        public Guid? EmployeeId { get; set; }

        //public int Serial { set; get; } 
        //public DateTime Time { get; set; } 
        //public string Content { get; set; } 
        //public decimal Cost { get; set; } 


    }

}