using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Models.DataBean
{
    [Comment("任务信息")]
    public class TaskCustomer : BaseCustomer
    {
        [Key, Comment("客户ID")]
        public override Guid CustomerId { get; set; } = Guid.NewGuid();
        [Comment("客户名称"), Required]
        public override string CustomerName { get; set; }
        [Comment("客户联系方式")]
        public override string? CustomerContactWay { get; set; }
        [Comment("客户地址")]
        public override string? CustomerAddress { get; set; }
        [Comment("客户类型"), ForeignKey("CategoryId")]
        public override Guid CustomerType { get; set; }

        [Comment("客户等级"), Range(1, 10, ErrorMessage = "客户等级必须在1-10之间")]
        public override int ClientGrade { get; set; }

        [Comment("客户添加时间")]
        public override DateTime AddTime { get; set; } = DateTime.Now;

        public Category? Category { get; set; }

        public TaskCustomer() { }

        public TaskCustomer(ICustomerInfo customer, Guid cateId) : base(customer, cateId) { }

        public TaskCustomer(BaseCustomer customer)
        {
            this.CustomerId = customer.CustomerId;
            this.CustomerName = customer.CustomerName;
            this.CustomerContactWay = customer.CustomerContactWay;
            this.CustomerAddress = customer.CustomerAddress;
            this.CustomerType = customer.CustomerType;
            this.ClientGrade = customer.ClientGrade;
            this.AddTime = customer.AddTime;
        }

        public TaskCustomer(string name, Guid cateId, int level = 1, string? conway = null, string? add = null)
        {
            CustomerId = Guid.NewGuid();
            CustomerName = name;
            CustomerContactWay = conway;
            CustomerAddress = add;
            CustomerType = cateId;
            ClientGrade = level;
            AddTime = DateTime.Now;

        }

        public void update(BaseCustomerInfo info)
        {
            CustomerAddress = info.CustomerAddress;
            CustomerContactWay = info.CustomerContactWay;
        }
    }

    [Comment("任务信息")]
    public class TaskAffair
    {
        [Key, Comment("任务ID")]
        public Guid TaskId { get; set; } = Guid.NewGuid();
        [Comment("任务创建时间")]
        public DateTime Time { get; set; } = DateTime.Now;

        [ForeignKey("CustomerId"), Comment("客户ID")]
        public Guid? CustomerId { get; set; }
        [Comment("任务描述"), Required]
        public string Content { get; set; }
        [ForeignKey("CategoryId"),Comment("任务类型")]
        public Guid TaskType{set;get;}
        [Range(0.0, 3000.0, ErrorMessage = "任务花费必须在0-3000之间")]
        public decimal Cost { get; set; } = 0.0M;
        [ForeignKey("EmployeeId"), Comment("发布者-员工ID")]
        public Guid EmployeeId { get; set; }


        public TaskCustomer TaskCustomer { get; set; }
        public EmployeeAccount EmployeeAccount { get; set; }

    }

    [Comment("人气跟踪情况")]
    public class TaskStatusTrack
    {
        [Key, Comment("任务跟踪ID")]
        public Guid TaskSatusId { get; set; }=Guid.NewGuid();
        [Comment("任务状态"),Range(0,7,ErrorMessage ="任务状态只能有[0:发布,1:进行中,2:完成,3:取消,4:过期,5:暂停,6:转让,7:未知]")]
        public int TaskStatus { get; set; }

        public DateTime TaskTrackTime { get; set; }=DateTime.Now;
        [ForeignKey("EmployeeId"),Comment("员工ID")]
        public Guid EmployeeId { get; set; }
        [Comment("任务ID"),ForeignKey("TaskId")]
        public Guid TaskId { get; set; }
        [Comment("备注")]
        public string? Remark { get; set; }



        public TaskAffair Task { get; set; }
        public EmployeeAccount EmployeeAccount { get; set; }

    }
}