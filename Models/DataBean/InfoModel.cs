
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Models.DataBean
{
    [Comment("分类信息")]
    public class Category : BaseCategory
    {
        [Key,Comment("分类ID"),ForeignKey("CategoryId")]
        public override Guid CategoryId { get; set; }=Guid.NewGuid();
        [Comment("父分类ID"),]
        public override Guid? ParentCategoryId { get; set; }

        [Comment("序列号"),DefaultValue(101)]
        public override int SortSerial { set; get; }

        [Comment("分类名称")]
        public override string CategoryName { get; set; }

        [Comment("分类等级"),DefaultValue(1),Range(1,20,ErrorMessage ="分类等级必须在1-20之间")]
        public override int CategoryLevel { get; set; }
        [Comment("备注")]
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

    [Comment("库存信息")]
    public class InventoryInfo
    {
        [Key,Comment("产品ID")]
        public Guid ProductId { get; set; }=Guid.NewGuid();
        
        [Required,Comment("产品名称")]
        public string ProductName { get; set; }
        [Comment("产品进货价"),Range(0.0,double.MaxValue,ErrorMessage ="最低进货价格必须大于等于0")]
        public decimal ProductPrice { get; set; }

        [Comment("产品零售价"),Range(0.0,double.MaxValue,ErrorMessage ="最低零售价格必须大于等于0")]
        public decimal ProductCost { get; set; }
        [Required,Comment("产品型号")]
        public string ProductModel { get; set; }
        [ForeignKey("CategoryId"),Comment("产品类型-使用分类ID")]
        public Guid ProductType { get; set; }


        public Category Category { get; set; }

    }
    [Comment("库存变动信息")]
    public class InOutStock
    {
        [Key,Comment("库存变动ID")]
        public Guid ListId { get; set; }=Guid.NewGuid();
        [Comment("库存变动时间")]
        public DateTime ChangeTime { get; set; }=DateTime.Now;
        [Comment("库存变动类型"),Range(0,2,ErrorMessage ="库存变动类型必须在[0:入库,1:出库,2:未知]之间")]
        public int InOutStockType { get; set; }
        [Comment("库存变动数量"),Range(1,255,ErrorMessage ="库存变动数量必须在1-254之间")]
        public int MaterialQuantity { get; set; }=1;
        [Comment("备注")]
        public string? Remark { get; set; }

        [ForeignKey("ProductId"),Comment("产品ID")]
        public Guid ProductId { get; set; }
        
        [ForeignKey("TaskId"),Comment("任务ID")]
        public Guid TaskId { get; set; }

        public TaskAffair Task { get; set; }


        public InventoryInfo InventoryInfo { get; set; }

    }
}