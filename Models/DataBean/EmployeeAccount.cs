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
        public class EmployeeAccount : BaseEmployee,IEmployee
        {
                [Key, Column("employee_electronic_account_id")]
                public new Guid EmployeeId { get; set; }

                [Column("alias"), Required]
                public new string EmployeeAlias { get; set; }

                [Column("employee_pwd"), Required]
                public new string EmployeePwd { get; set; }

                [Column("account_permission")]
                public new int AccountPermission { get; set; }

                public EmployeeAccount() { 
                }

                public EmployeeAccount(Info obj,Guid id,string pwd) {
                        this.EmployeeId = id;
                        this.EmployeeAlias = obj.EmployeeAlias;
                        this.EmployeePwd = pwd;
                        this.AccountPermission = obj.AccountPermission;
                }

                public EmployeeAccount(AliasAccount obj, Guid id)
                {
                        this.EmployeeId = id;
                        this.EmployeeAlias = obj.EmployeeAlias;
                        this.EmployeePwd = obj.EmployeePwd;
                        this.AccountPermission = obj.AccountPermission;
                }

                public EmployeeAccount(Part obj,Guid id){
                         this.EmployeeId = id;
                        this.EmployeeAlias = obj.EmployeeAlias;
                        this.EmployeePwd = obj.EmployeePwd;
                        this.AccountPermission = 1;
                }
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