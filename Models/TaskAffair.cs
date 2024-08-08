using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskManangerSystem.IServices;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Repository;

namespace TaskManangerSystem.Models
{
    [Comment("任务信息")]
    public class TaskAffair : ITaskAttair, IUpdateable<TaskAffairForUpdate>
    {
        [Key, Comment("任务ID")]
        public Guid TaskId { get; set; } = Guid.NewGuid();

        public int Serial { set; get; } = 0;
        [Comment("任务创建时间")]
        public DateTime Time { get; set; } = DateTime.Now;


        [Comment("任务描述")]
        public string Content { get; set; } = string.Empty;

        [Comment("任务花费")]
        public decimal Cost { get; set; } = 0.0M;

        [Comment("任务类型")]
        public Guid TaskType { set; get; }

        [Comment("客户ID")]
        public Guid CustomerId { get; set; }

        [Comment("发布者-员工ID")]
        public Guid EmployeeId { get; set; }

        public virtual Customer? Customers { get; set; }
        public virtual Category? Categorys { get; set; }
        public virtual Employee? EmployeeAccounts { get; set; }

        public TaskAffair(TaskAffairForAdd add, Category taskCategory, Customer customer, Employee employee, int serial = 0)
        {
            Content = add.Content;
            Cost = add.Cost;
            Serial = serial;

            Categorys = taskCategory;
            TaskType = taskCategory.CategoryId;
            Customers = customer;
            CustomerId = customer.CustomerId;
            EmployeeAccounts = employee;
            EmployeeId = employee.EmployeeId;
        }

        public void Update(TaskAffairForUpdate af)
        {
            Cost = af.Cost;
            Content = af.Content;
        }

        public TaskAffair() { }

    }

    [Comment("任务跟踪情况")]
    public class TaskStatusTrack
    {
        [Key, Comment("任务跟踪ID")]
        public Guid TaskSatusId { get; set; } = Guid.NewGuid();
        [Comment("任务状态")]
        public int TaskStatus { get; set; }

        public DateTime TaskTrackTime { get; set; } = DateTime.Now;
        [Comment("员工ID")]
        public Guid EmployeeId { get; set; }
        [Comment("任务ID")]
        public Guid TaskId { get; set; }
        [Comment("备注")]
        public string? Remark { get; set; }

        public virtual TaskAffair? Tasks { get; set; }
        public virtual Employee? EmployeeAccounts { get; set; }

    }
}