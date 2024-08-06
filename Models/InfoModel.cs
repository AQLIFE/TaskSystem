
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServices;

namespace TaskManangerSystem.Models
{
    [Comment("分类信息")]
    public class Category : ICategory
    {
        [Key, Comment("分类ID")]
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        [Comment("父分类ID"), ForeignKey("CategoryId")]
        public Guid? ParentCategoryId { get; set; }

        [Comment("序列号"), DefaultValue(101)]
        public int SortSerial { set; get; }

        [Comment("分类名称")]
        public string CategoryName { get; set; } = string.Empty;

        [Comment("分类等级"), DefaultValue(1), Range(1, 20, ErrorMessage = "分类等级必须在1-20之间")]
        public int CategoryLevel { get; set; }
        [Comment("备注")]
        public string? Remark { get; set; }

        public virtual Category? ParentCategory { get; set; }

        public Category() { }

        //public void Update() { }





        // 用于系统初始化
        public Category(string name, int serial, string? remark = null, int level = 1, Guid? parId = null)
        {
            CategoryName = name;
            SortSerial = serial;
            Remark = remark;
            CategoryLevel = level;
            ParentCategoryId = parId;
        }
    }


    [Comment("库存信息")]
    public class InventoryInfo : IInventoryInfo
    {
        [Key, Comment("产品ID")]
        public Guid ProductId { get; set; } = Guid.NewGuid();

        [Required, Comment("产品名称")]
        public string ProductName { get; set; } = string.Empty;
        [Comment("产品进货价"), Range(0.0, double.MaxValue, ErrorMessage = "最低进货价格必须大于等于0")]
        public decimal ProductPrice { get; set; }

        [Comment("产品零售价"), Range(0.0, double.MaxValue, ErrorMessage = "最低零售价格必须大于等于0")]
        public decimal ProductCost { get; set; }
        [Required, Comment("产品型号")]
        public string ProductModel { get; set; } = string.Empty;
        [Comment("产品类型-使用分类ID"), ForeignKey("CategoryId")]
        public Guid? ProductType { get; set; }

        public virtual Category? Categories { get; set; }

        public InventoryInfo() { }

        public InventoryInfo(Guid productId, string productName, decimal productPrice, decimal productCost, string productModel, Category? categories)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
            ProductCost = productCost;
            ProductModel = productModel;
            Categories = categories;
            ProductType = categories?.CategoryId;
        }
    }

    [Comment("库存变动信息")]
    public class InOutStock
    {
        [Key, Comment("库存变动ID")]
        public Guid ListId { get; set; } = Guid.NewGuid();
        [Comment("库存变动时间")]
        public DateTime ChangeTime { get; set; } = DateTime.Now;
        [Comment("库存变动类型"), Range(0, 2, ErrorMessage = "库存变动类型必须在[0:入库,1:出库,2:未知]之间")]
        public int InOutStockType { get; set; }
        [Comment("库存变动数量"), Range(1, 255, ErrorMessage = "库存变动数量必须在1-254之间")]
        public int MaterialQuantity { get; set; } = 1;
        [Comment("备注")]
        public string? Remark { get; set; }



        [Comment("任务ID")]
        public Guid? TaskId { get; set; }


        public virtual TaskAffair? Task { get; set; }


        public virtual InventoryInfo? InventoryInfo { get; set; }

        [Comment("产品ID")]
        public Guid? ProductId { get; set; }

    }
}