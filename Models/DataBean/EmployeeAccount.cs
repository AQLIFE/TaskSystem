using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Models.DataBean
{
        [Comment("员工账户信息")]
        public class EmployeeAccount : BaseEmployee, IEmployee
        {
                [Key,Comment("GUID"),Required(ErrorMessage ="员工电子账户ID不能为空")]
                public override Guid EmployeeId { set;get;}=Guid.NewGuid();

                [Comment("账户名称"), Required(ErrorMessage="账户名不可为空"),ConcurrencyCheck,RegularExpression(@"^[\dA-Za-z]{5,16}$", ErrorMessage = "员工ID必须是普通字符[数字|字母(不区分大小写)]"),StringLength(16,MinimumLength =5,ErrorMessage ="账户名长度必须控制在5-16位字符")]
                public override string EmployeeAlias { get; set; }

                [Comment("账户密码"),Required(ErrorMessage ="密码不可为空"),ConcurrencyCheck,MinLength(8,ErrorMessage ="密码长度不足")]
                public override string EmployeePwd { get; set; }

                [ Comment("账户权限"),Required(ErrorMessage ="权限级不能为空"),ConcurrencyCheck,DefaultValue(1),Range(0,100,ErrorMessage ="权限不能低于0")]
                public override int AccountPermission { get; set; }=1;
                // 当权限为0时，视为封存账户

                #region 注参实现
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

                #endregion
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

        [Comment("员工信息")]
        public class EmployeeInfo
        {
                [Key,ForeignKey("EmployeeId"),Comment("员工ID")]
                public Guid EmployeeId { get; set; }
                [MinLength(2,ErrorMessage ="姓名长度必须大于等于2"),Comment("员工姓名")]
                public string? EmployeeName { get; set; }
                [MinLength(11,ErrorMessage ="手机号长度必须大于等于11"),Comment("员工联系方式")]
                public string? EmployeeContactWay { get; set; }
                
                // [Required]
                public EmployeeAccount EmployeeSystemAccount { get; set; }
        }
}