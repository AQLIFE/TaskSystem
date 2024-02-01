using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServer;

namespace TaskManangerSystem.Models
{
        /// <summary>
        /// 员工加密数据视图
        /// 返回加密视图的数据，仅用于浏览，不可对其进行修改和提交数据
        /// </summary>
        [Table("alias_md5")]
        public class AliasMd5 : AEmployeeToAlias, IAlias
        {
                [Column("id")]
                public Guid Id{get;set;}

                [Column("encryption_id")]
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
                public string EncryptionId { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

                [Column("alias")]
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
                public string EmployeeAlias { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

                [Column("pwd")]
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
                public string EmployeePwd { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

                [Column("account_permission")]
                public int AccountPermission { get; set; }


                public override AliasEmployeeSystemAccount ToAlias() => new AliasEmployeeSystemAccount(this);

                public override AliasEmployeeSystemAccount ToAlias(char cr) => new AliasEmployeeSystemAccount(this, cr);
        }
}
