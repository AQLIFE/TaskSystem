using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServer;

namespace TaskManangerSystem.Models
{
        /// <summary>
        /// 员工电子账户表
        /// 对应数据库的真实数据，无加密和遮蔽
        /// </summary>
        [Table("employee_system_account_table")]
        public class EmployeeSystemAccount :  IEmployee
        {
                [Key, Column("employee_electronic_account_id")]
                public Guid EmployeeId { get; set; }

                [Column("alias"), Required]
                public string EmployeeAlias { get; set; }

                [Column("employee_pwd"), Required]
                public string EmployeePwd { get; set; }

                [Column("account_permission")]
                public int AccountPermission { get; set; }

                public EmployeeSystemAccount(AliasEmployeeSystemAccount obj)
                {
                        this.EmployeeId = Guid.NewGuid();
                        this.EmployeeAlias = obj.EmployeeAlias;
                        this.EmployeePwd = obj.EmployeePwd;
                        this.AccountPermission = obj.AccountPermission;
                }

                public EmployeeSystemAccount(Guid id,AliasEmployeeSystemAccount obj)
                {
                        this.EmployeeId = id;
                        this.EmployeeAlias = obj.EmployeeAlias;
                        this.EmployeePwd = obj.EmployeePwd;
                        this.AccountPermission = obj.AccountPermission;
                }


                public EmployeeSystemAccount() { }


                [Obsolete("该方法会暴露真实数据，请谨慎使用")]
                public AliasEmployeeSystemAccount ToAlias() => new (this);
                
                
        }

        [Table("employee_real_info_table")]
        public class EmployeeRealInfo
        {
                [Key]
                public Guid EmployeeElectronicAccountId { get; set; }

                public string EmployeeName { get; set; }


                public string EmployeeContactWay { get; set; }



                public EmployeeSystemAccount EmployeeSystemAccount { get; set; }

        }
}