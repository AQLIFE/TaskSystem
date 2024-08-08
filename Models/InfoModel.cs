
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServices;

namespace TaskManangerSystem.Models
{
    [Comment("库存信息")]
    public class InventoryInfo : IInventoryInfo
    {
        [Key, Comment("产品ID")]
        public Guid ProductId { get; set; } = Guid.NewGuid();

        [Comment("产品名称")]
        public string ProductName { get; set; } = string.Empty;
        [Comment("产品进货价")]
        public decimal ProductPrice { get; set; }

        [Comment("产品零售价")]
        public decimal ProductCost { get; set; }
        [Comment("产品型号")]
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
        [Comment("库存变动类型")]
        public int InOutStockType { get; set; }
        [Comment("库存变动数量")]
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