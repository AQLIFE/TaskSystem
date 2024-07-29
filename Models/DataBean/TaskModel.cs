using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Models.DataBean
{
    [Comment("任务信息")]
    public class Customer : ICustomer
    {
        [Key, Comment("客户ID")]
        public Guid CustomerId { get; set; } = Guid.NewGuid();
        [Comment("客户名称"), Required]
        public string CustomerName { get; set; } = string.Empty;

        //[NotMapped]
        //public string HashName => ShaHashExtensions.ComputeSHA256Hash(CustomerName);
        [Comment("客户联系方式")]
        public string? CustomerContactWay { get; set; }
        [Comment("客户地址")]
        public string? CustomerAddress { get; set; }
        [Comment("客户类型")]
        public Guid? CustomerType { get; set; }

        [Comment("客户等级"), Range(1, 10, ErrorMessage = "客户等级必须在1-10之间")]
        public int ClientGrade { get; set; } = 1;

        [Comment("客户添加时间")]
        public DateTime AddTime { get; set; } = DateTime.Now;

        public virtual Category? Categories { get; set; }

        public Customer() { }
        public Customer(Category? cate/*,CustomerForAddOrUpdate info*/)
        {
            //CustomerName = info.CustomerName;
            //CustomerAddress = info.CustomerAddress;
            //CustomerContactWay = info.CustomerContactWay;
            //Categories = cate;
            CustomerType = cate?.CategoryId;
        }

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

    [Comment("任务信息")]
    public class TaskAffair : ITaskAttair, IUpdateable<TaskAffairForUpdate>
    {
        [Key, Comment("任务ID")]
        public Guid TaskId { get; set; } = Guid.NewGuid();

        public int Serial { set; get; } = 0;
        [Comment("任务创建时间")]
        public DateTime Time { get; set; } = DateTime.Now;


        [Comment("任务描述"), Required]
        public string Content { get; set; } = string.Empty;

        [Range(0.0, 3000.0, ErrorMessage = "任务花费必须在0-3000之间")]
        public decimal Cost { get; set; } = 0.0M;

        [Comment("任务类型")]
        public Guid? TaskType { set; get; }

        [Comment("客户ID")]
        public Guid? CustomerId { get; set; }

        [Comment("发布者-员工ID")]
        public Guid? EmployeeId { get; set; }

        public virtual Customer? Customers { get; set; }
        public virtual Category? Categorys { get; set; }
        public virtual EmployeeAccount? EmployeeAccounts { get; set; }

        public TaskAffair(Category? taskCategory, Customer? customer, EmployeeAccount? employee)
        {
            Categorys = taskCategory;
            TaskType = taskCategory?.CategoryId;
            Customers = customer;
            CustomerId = customer?.CustomerId;
            EmployeeAccounts = employee;
            EmployeeId = employee?.EmployeeId;
        }

        public void Update(TaskAffairForUpdate af)
        {
            this.Cost = af.Cost;
            this.Content = af.Content;
            //return true;

        }

        public TaskAffair() { }

    }

    [Comment("任务跟踪情况")]
    public class TaskStatusTrack
    {
        [Key, Comment("任务跟踪ID")]
        public Guid TaskSatusId { get; set; } = Guid.NewGuid();
        [Comment("任务状态"), Range(0, 7, ErrorMessage = "任务状态只能有[0:发布,1:进行中,2:完成,3:取消,4:过期,5:暂停,6:转让,7:未知]")]
        public int TaskStatus { get; set; }

        public DateTime TaskTrackTime { get; set; } = DateTime.Now;
        [Comment("员工ID")]
        public Guid EmployeeId { get; set; }
        [Comment("任务ID")]
        public Guid TaskId { get; set; }
        [Comment("备注")]
        public string? Remark { get; set; }

        public virtual TaskAffair? Tasks { get; set; }
        public virtual EmployeeAccount? EmployeeAccounts { get; set; }

    }
}