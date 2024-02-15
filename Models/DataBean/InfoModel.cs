
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManangerSystem.Models.DataBean
{
    [Table("category_table")]
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        public Guid? ParentCategoryId { get; set; }

        public string CategoryName { get; set; }

        public int CategoryLevel { get; set; }

        public string Remark { get; set; }

        public Category ParentCategory { get; set; }

    }

    public class InventoryInfo
    {
        [Key]
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }
        public decimal ProductCost { get; set; }

        public string ProductModel { get; set; }

        public Guid ProductType { get; set; }


        public Category Category { get; set; }

    }

    public class InOutStock
    {
        [Key]
        public Guid ListId { get; set; }
        public DateTime ChangeTime { get; set; }

        public string InOutStockType { get; set; }

        public int MaterialQuantity { get; set; }

        public string Remark { get; set; }

        public Guid ProductId { get; set; }
        public Guid TaskId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid EmployeeId { get; set; }


        public TaskAffair Task { get; set; }


        public InventoryInfo InventoryInfo { get; set; }


        public TaskCustomer TaskCustomer { get; set; }


        public EmployeeAccount EmployeeSystemAccount { get; set; }

    }
}