using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServer;

namespace TaskManangerSystem.Models
{
        /// <summary>
        /// 员工加密数据视图
        /// 返回加密视图的数据，仅用于浏览，不可对其进行修改和提交数据
        /// </summary>
        [Table("alias_md5")]
        public class EncryptEmployeeSystemAccount
        {
                [Column("id")]
                public Guid Id{get;set;}

                [Column("encryption_id")]

                public string EncryptionId { get; set; }


                [Column("alias")]

                public string EmployeeAlias { get; set; }


                [Column("pwd")]

                public string EmployeePwd { get; set; }


                [Column("account_permission")]
                public int AccountPermission { get; set; }
        }
}
