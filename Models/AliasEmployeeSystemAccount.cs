using System.ComponentModel.DataAnnotations;
using TaskManangerSystem.IServer;
using System.Collections.Generic;

namespace TaskManangerSystem.Models
{
    /// <summary>
    /// 用于返回脱敏数据
    /// </summary>
    public class AliasEmployeeSystemAccount
    {


        [Required]
        public string EmployeeAlias { get; set; }
        [Required]
        public string EmployeePwd { get; set; }

        public int AccountPermission { get; set; }

        public AliasEmployeeSystemAccount() { }
        public EmployeeSystemAccount ToEmployeeSystemAccount() => new EmployeeSystemAccount(this);
        public EmployeeSystemAccount ToEmployeeSystemAccount(Guid id) => new EmployeeSystemAccount(id,this);

        /// <summary>
        /// 截取真实数据，未加密，未遮蔽
        /// </summary>
        /// <param name="obj"></param>
        public AliasEmployeeSystemAccount(EmployeeSystemAccount obj)
        {
            // 舍弃 Id
            this.EmployeeAlias = obj.EmployeeAlias;
            this.EmployeePwd = obj.EmployeePwd;
            this.AccountPermission = obj.AccountPermission;
        }

        /// <summary>
        /// 返回加密数据
        /// </summary>
        /// <param name="obj"></param>
        public AliasEmployeeSystemAccount(EncryptEmployeeSystemAccount obj)
        {

            this.EmployeeAlias = obj.EmployeeAlias;
            this.EmployeePwd = obj.EmployeePwd;
            this.AccountPermission = obj.AccountPermission;
        }
        /// <summary>
        /// 返回遮蔽的数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cr"></param>
        public AliasEmployeeSystemAccount(EmployeeSystemAccount obj, char cr)
        {
            this.EmployeeAlias = obj.EmployeeAlias;
            this.EmployeePwd = new string(cr == default ? '*' : cr, 10);
            this.AccountPermission = obj.AccountPermission;
        }

        public AliasEmployeeSystemAccount(EncryptEmployeeSystemAccount obj, char cr)
        {
            this.EmployeeAlias = obj.EmployeeAlias;
            this.EmployeePwd = new string(cr == default ? '*' : cr, 10);
            this.AccountPermission = obj.AccountPermission;
        }

        // public AliasEmployeeSystemAccount<T>(T obj,char cr){
        //     if (typeof(obj) == AliasMd5 || typeof(obj)==EmployeeSystemAccount  )
        //     {

        //     }
        //     this.EmployeeAlias = obj.EmployeeAlias;
        //     this.EmployeePwd = new string(cr == default ? '*' : cr, 10);
        //     this.AccountPermission = obj.AccountPermission;
        // }
    }
}