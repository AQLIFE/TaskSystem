using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManangerSystem.Models.DataBean
{
    public class TaskCustomer
    {
        [Key]
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string ?CustomerContactWay { get; set; }
        public string ?CustomerAddress { get; set; }
        public Guid CustomerType { get; set; }

        public Category ?Category { get; set; }
    }

    [Table("task_table")]
    public class Task
    {
        [Key]
        public Guid TaskId { get; set; }
        public DateTime Time { get; set; }
        public Guid? CustomerId { get; set; }

        public string Content { get; set; }

        public decimal Cost { get; set; }
        public Guid EmployeeId { get; set; }


        public TaskCustomer TaskCustomer { get; set; }


        public EmployeeSystemAccount EmployeeSystemAccount { get; set; }

    }

    public class TaskStatusTrack
    {
        [Key,Column("task_id")]
        public Guid TaskSatusId { get; set; }

        public string TaskStatus { get; set; }

        public DateTime TaskTrackTime { get; set; }
        public Guid EmployeeId { get; set; }

        public string Remark { get; set; }



        public Task Task { get; set; }


        public EmployeeSystemAccount EmployeeSystemAccount { get; set; }

    }
}