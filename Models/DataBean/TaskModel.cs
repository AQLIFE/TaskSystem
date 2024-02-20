using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManangerSystem.Models.DataBean
{
    [Table("task_customer_table")]
    public class TaskCustomer
    {
        [Key, Column("customer_id")]
        public Guid CustomerId { get; set; }
        [Column("customer_name")]
        public string CustomerName { get; set; }
        [Column("customer_contact_way")]
        public string? CustomerContactWay { get; set; }
        [Column("customer_address")]
        public string? CustomerAddress { get; set; }
        [Column("customer_type"),ForeignKey("Category")]
        public Guid CustomerType { get; set; }

        public Category? Category { get; set; }
    }

    [Table("task_table")]
    public class TaskAffair
    {
        [Key]
        public Guid TaskId { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("TaskCustomer"),Column("customer_id")]
        public Guid? CustomerId { get; set; }

        public string Content { get; set; }

        public decimal Cost { get; set; }
        public Guid EmployeeId { get; set; }


        public TaskCustomer TaskCustomer { get; set; }


        public EmployeeAccount EmployeeAccount { get; set; }

    }

    public class TaskStatusTrack
    {
        [Key, Column("task_id")]
        public Guid TaskSatusId { get; set; }

        public string TaskStatus { get; set; }

        public DateTime TaskTrackTime { get; set; }
        public Guid EmployeeId { get; set; }

        public string Remark { get; set; }



        public TaskAffair Task { get; set; }


        public EmployeeAccount EmployeeAccount { get; set; }

    }
}