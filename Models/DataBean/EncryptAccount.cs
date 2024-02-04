using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServices.BeanServices;

namespace TaskManangerSystem.Models.DataBean
{
        /// <summary>
        /// 员工加密数据视图
        /// 返回加密视图的数据，仅用于浏览，不可对其进行修改和提交数据
        /// </summary>
        
        [Table("alias_md5")]
        public class EncryptAccount : BaseEncrypt,IEncrypt
        {
                [Column("id")]
                public new Guid EmployeeId{get;set;}

                [Column("encryption_id")]
                public new string EncryptionId{ get; set; }


                [Column("alias")]
                public new string EmployeeAlias { get; set; }

                [Column("pwd")]
                public new string EmployeePwd { get; set; }


                [Column("account_permission")]
                public new int AccountPermission { get; set; }
        }
}
