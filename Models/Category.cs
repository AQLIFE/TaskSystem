using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServices;
using TaskManangerSystem.Services.Tool;

namespace TaskManangerSystem.Models
{
    [Comment("分类信息")]
    public class Category : ICategory
    {
        [Key, Comment("分类ID")]
        public Guid CategoryId { get; set; } = Guid.NewGuid();

        [Comment("父分类ID"), ForeignKey("CategoryId")]
        public Guid? ParentCategoryId { get; set; }

        [Comment("序列号")]
        public int SortSerial { set; get; } = SystemInfo.EMPLOYEE;

        [Comment("分类名称")]
        public string CategoryName { get; set; } = string.Empty;

        [Comment("分类等级")]
        public int CategoryLevel { get; set; }
        
        [Comment("备注")]
        public string? Remark { get; set; }

        public virtual Category? ParentCategory { get; set; }

        public Category() { }

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
}
