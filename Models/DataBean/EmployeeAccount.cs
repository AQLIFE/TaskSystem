using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Models.DataBean
{
        /// <summary>
        /// 员工电子账户表
        /// 对应数据库的真实数据，无加密和遮蔽
        /// </summary>
        [Table("employee_system_account_table")]
        public class EmployeeAccount : BaseEmployee, IEmployee
        {
                [Key, Column("employee_electronic_account_id")]
                public override Guid EmployeeId { get; set; }

                [Column("alias"), Required]
                public override string EmployeeAlias { get; set; }

                [Column("employee_pwd"), Required]
                public override string EmployeePwd { get; set; }

                [Column("account_permission")]
                public override int AccountPermission { get; set; }

                public EmployeeAccount(IPart obj):base(obj){}
                public EmployeeAccount(IPartInfo obj,string pwd,Guid id):base(obj,pwd,id){}

                public EmployeeAccount(){}
                public EmployeeAccount(string name,string pwd,int ap=1){
                        EmployeeId = Guid.NewGuid();
                        EmployeeAlias = name;
                        EmployeePwd = pwd;
                        AccountPermission = ap;
                }

                public override BasePartInfo ToBasePartInfo()=>new BasePartInfo(this);

                // public EmployeeAccount(Info obj, Guid id, string pwd)
                // {
                //         this.EmployeeId = id;
                //         this.EmployeeAlias = obj.EmployeeAlias;
                //         this.EmployeePwd = pwd;
                //         this.AccountPermission = obj.AccountPermission;
                // }

                // public EmployeeAccount(Part obj, Guid id)
                // {
                //         this.EmployeeId = id;
                //         this.EmployeeAlias = obj.EmployeeAlias;
                //         this.EmployeePwd = obj.EmployeePwd;
                //         this.AccountPermission = 1;
                // }
        }

        [Table("employee_real_info_table")]
        public class EmployeeRealInfo
        {
                [Key]
                public Guid EmployeeElectronicAccountId { get; set; }
                public string EmployeeName { get; set; }
                public string EmployeeContactWay { get; set; }
                public EmployeeAccount EmployeeSystemAccount { get; set; }
        }
}