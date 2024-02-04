using System.ComponentModel.DataAnnotations;
using TaskManangerSystem.IServices.BeanServices;
using System.Collections.Generic;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Models.SystemBean
{
    /// <summary>
    /// 用于返回脱敏数据
    /// </summary>
    public class AliasEmployeeSystemAccount : BaseAlias
    {
        public AliasEmployeeSystemAccount() { }
        public EmployeeSystemAccount ToEmployeeSystemAccount() => new (this,Guid.NewGuid());
        public EmployeeSystemAccount ToEmployeeSystemAccount(Guid id) => new (this,id);

        #region 类型转换
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

        // /// <summary>
        // /// 返回加密数据
        // /// </summary>
        // /// <param name="obj"></param>
        public AliasEmployeeSystemAccount(EncryptEmployeeSystemAccount obj)
        {

            this.EmployeeAlias = obj.EmployeeAlias;
            this.EmployeePwd = obj.EmployeePwd;
            this.AccountPermission = obj.AccountPermission;
        }
        // /// <summary>
        // /// 返回遮蔽的数据
        // /// </summary>
        // /// <param name="obj"></param>
        // /// <param name="cr"></param>
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
        #endregion
    }
    public class AliasConverter <T>  where T :IEmployee
    {
        // private T _obj;

        public static AliasEmployeeSystemAccount Result;

        public AliasConverter(T value)
        {
            // _obj = value;
            Result = ConvertToAlias(value);
        }

        private AliasEmployeeSystemAccount ConvertToAlias(IAlias alias)
        {
            switch (alias)
            {
                case EncryptEmployeeSystemAccount encrypt:
                    return new AliasEmployeeSystemAccount(encrypt);
                case EmployeeSystemAccount account:
                    return new AliasEmployeeSystemAccount(account);
                default:
                    throw new ArgumentException("Unsupported type for conversion.");
            }
        }
    }
}