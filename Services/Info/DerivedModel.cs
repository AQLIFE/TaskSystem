using System.ComponentModel.DataAnnotations;
using TaskManangerSystem.IServices;
using TaskManangerSystem.Services.Attriabute;

namespace TaskManangerSystem.Services.Info
{
    /// <summary>
    /// 用于返回脱敏数据
    /// </summary>
    public class EmployeeAccountForLoginOrAdd() : IPart
    {
        [EntityValidator(alias:nameof(EmployeeAlias),pattern: @"^[a-z\d]{5,16}$", errorMessage: "账户ID必须是普通字符[数字|小写字母]{5-16位}")]
        public string EmployeeAlias { get; set; } = string.Empty;

        [EntityValidator(alias:nameof(EmployeePwd),pattern: @"^[\dA-Za-z]{8,128}$", errorMessage:"账户密码必须是普通字符[数字|字母]{8-128位}")]
        public string EmployeePwd { get; set; } = string.Empty;
    }

    public class EmployeeAccountForSelectOrUpdate() : IPartInfo
    {
        public string EmployeeAlias { get; set; } = string.Empty;
        public int AccountPermission { get; set; } = 0;
    }


    public class CategoryForSelect() : ICategoryInfo
    {
        public int SortSerial { set; get; }
        public int ParentSortSerial { set; get; }// 子类自定义
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryLevel { get; set; }
        public string? Remark { get; set; }
    }

    public class CategoryForAddOrUpdate()
    {
        public int ParentSortSerial { set; get; }// 子类自定义
        public string CategoryName { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }

    public class CustomerForAddOrUpdate() : ICustomerInfo
    {
        public virtual string CustomerName { get; set; } = string.Empty;
        public virtual string? CustomerContactWay { get; set; }
        public virtual string? CustomerAddress { get; set; }
        public virtual string? CustomerType { get; set; } = string.Empty;
    }

    public class CustomerForSelect() : CustomerForAddOrUpdate
    {
        public int ClientGrade { get; set; }
        public DateTime AddTime { get; set; }
    }

    public class InventoryForAddOrUpdate() : IInventory
    {
        public string ProductName { get; set; } = string.Empty;

        public decimal ProductPrice { get; set; }

        public decimal ProductCost { get; set; }

        public string ProductModel { get; set; } = string.Empty;

        public string? ProductType { get; set; } = string.Empty;

    }


    public class TaskAffairForSelect : TaskAffairForAdd
    {
        public int Serial { set; get; }
        public DateTime Time { get; set; }
        //public string? Content { get; set; } = string.Empty;
        //public decimal Cost { get; set; }

        //public string? TaskType { get; set; } = string.Empty;
        //public string? CustomerName { set; get; } = string.Empty;
        //public string? EmployeeName { set; get; } = string.Empty;

        public TaskAffairForSelect() { }
        public TaskAffairForSelect(string? type, string? cu, string? em) : base(type, cu, em) { }
    }

    public class TaskAffairForUpdate : ITaskAttair
    {
        public string Content { get; set; } = string.Empty;
        public decimal Cost { get; set; }

    }

    public class TaskAffairForAdd : TaskAffairForUpdate
    {
        [EntityValidator(nameof(TaskType),required:true)]
        public string TaskType { get; set; } = string.Empty;

        [EntityValidator(nameof(CustomerName), required: true)]
        public string CustomerName { set; get; } = string.Empty;

        [EntityValidator(nameof(EmployeeName), required: true)]
        public string EmployeeName { set; get; } = string.Empty;

        public TaskAffairForAdd() { }

        public TaskAffairForAdd(string? type, string? cu, string? em)
        {
            TaskType = type;
            CustomerName = cu;
            EmployeeName = em;
        }
    }

}