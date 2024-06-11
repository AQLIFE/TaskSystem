using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Models.SystemBean
{
    /// <summary>
    /// 用于返回脱敏数据
    /// </summary>
    public class EmployeeAccountForLoginOrAdd() : IPart
    {
        public string EmployeeAlias { get; set; } = string.Empty;
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

    public class CategoryForAddOrUpdate
    {
        public int ParentSortSerial { set; get; }// 子类自定义
        public string CategoryName { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }

    public class CustomerForView() : ICustomerInfo
    {
        public virtual string CustomerName { get; set; } = string.Empty;
        public virtual string? CustomerContactWay { get; set; }
        public virtual string? CustomerAddress { get; set; }
        public virtual string? CustomerType { get; set; } = string.Empty;
    }

    public class CustomerForSelect() : CustomerForView
    {
        public int ClientGrade { get; set; }
        public DateTime AddTime { get; set; }
    }

    public class InventoryForView() : IInventory
    {       
        public string ProductName { get; set; } = string.Empty;
        
        public decimal ProductPrice { get; set; }

        public decimal ProductCost { get; set; }
        
        public string ProductModel { get; set; } = string.Empty;

        public string? ProductType { get; set; } = string.Empty;

    }

}