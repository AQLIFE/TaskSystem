using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskManangerSystem.IServices;

namespace TaskManangerSystem.Models
{
    [Comment("任务信息")]
    public class Customer : ICustomer
    {
        [Key, Comment("客户ID")]
        public Guid CustomerId { get; set; } = Guid.NewGuid();
        [Comment("客户名称")]
        public string CustomerName { get; set; } = string.Empty;


        [Comment("客户联系方式")]
        public string? CustomerContactWay { get; set; }
        [Comment("客户地址")]
        public string? CustomerAddress { get; set; }
        [Comment("客户类型")]
        public Guid? CustomerType { get; set; }

        [Comment("客户等级")]
        public int ClientGrade { get; set; } = 1;

        [Comment("客户添加时间")]
        public DateTime AddTime { get; set; } = DateTime.Now;

        public virtual Category? Categories { get; set; }

        public Customer() { }


        public Customer(string name, Guid cateId, int level = 1, string? conway = null, string? add = null)
        {
            CustomerId = Guid.NewGuid();
            CustomerName = name;
            CustomerContactWay = conway;
            CustomerAddress = add;
            CustomerType = cateId;
            ClientGrade = level;
            AddTime = DateTime.Now;

        }//init

    }
}
