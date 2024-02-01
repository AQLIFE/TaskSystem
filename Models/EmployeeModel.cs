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
        public class EmployeeSystemAccount : AEmployeeToAlias, IEmployee
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

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
                public EmployeeSystemAccount() { }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

                // [Obsolete("该方法会暴露真实数据，请谨慎使用")]
                public override AliasEmployeeSystemAccount ToAlias() => new AliasEmployeeSystemAccount(this);
                public override AliasEmployeeSystemAccount ToAlias(char cr) => new AliasEmployeeSystemAccount(this, cr);
        }

        [Table("employee_real_info_table")]
        public class EmployeeRealInfo
        {
                [Key]
                public Guid EmployeeElectronicAccountId { get; set; }
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
                public string EmployeeName { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
                public string EmployeeContactWay { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
                public EmployeeSystemAccount EmployeeSystemAccount { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        }
}