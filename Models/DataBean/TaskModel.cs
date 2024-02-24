using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Models.DataBean
{
    [Table("task_customer_table")]
    public class TaskCustomer :BaseCustomer
    {
        [Key, Column("customer_id")]
        public override Guid CustomerId { get; set; }
        [Column("customer_name")]
        public override string CustomerName { get; set; }
        [Column("customer_contact_way")]
        public override string? CustomerContactWay { get; set; }
        [Column("customer_address")]
        public override string? CustomerAddress { get; set; }
        [Column("customer_type"), ForeignKey("Category")]
        public override Guid CustomerType { get; set; }
        
        [Column("client_grade")]
        public override int ClientGrade { get; set; }

        [Column("add_time")]
        public override DateTime AddTime { get; set; }

        public Category? Category { get; set; }

        public TaskCustomer() { }

        public TaskCustomer(ICustomer customer,Guid cateId):base(customer,cateId){}

        public TaskCustomer(BaseCustomer customer){
            this.CustomerId = customer.CustomerId;
            this.CustomerName = customer.CustomerName;
            this.CustomerContactWay = customer.CustomerContactWay;
            this.CustomerAddress = customer.CustomerAddress;
            this.CustomerType = customer.CustomerType;
            this.ClientGrade = customer.ClientGrade;
            this.AddTime = customer.AddTime;
        }
    }

    [Table("task_table")]
    public class TaskAffair
    {
        [Key]
        public Guid TaskId { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("TaskCustomer"), Column("customer_id")]
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