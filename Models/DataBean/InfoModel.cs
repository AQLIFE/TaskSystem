
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Models.DataBean
{
    [Table("category_table")]
    public class Category : BaseCategory
    {
        [Key, Column("category_id")]
        public override Guid CategoryId { get; set; }
        [Column("parent_category_id")]
        public override Guid? ParentCategoryId { get; set; }

        [Column("sort_serial")]
        public override int SortSerial { set; get; }

        [Column("category_name")]
        public override string CategoryName { get; set; }

        [Column("category_level")]
        public override int CategoryLevel { get; set; }

        public override string? Remark { get; set; }

        public Category ParentCategory { get; set; }

        public Category(){}

        public Category(ICateInfo? cateInfo, Guid id, Guid? parId):base(cateInfo,id,parId){}
        public Category(CaInfo? cateInfo, Guid id, Guid? parId,int sort,int level):base(cateInfo,id,parId,sort,level){}
        
        public Category(string name,int serial=100,string? remark=null,int level=1,Guid? parId=null){
            CategoryId = Guid.NewGuid();
            CategoryName = name;
            SortSerial = serial;
            ParentCategoryId = parId;
            CategoryLevel=level;
            Remark = remark;
        }
        public BaseCateInfo ToCateInfo(int parId) => new BaseCateInfo(this, parId);

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