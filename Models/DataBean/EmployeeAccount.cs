using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices;

namespace TaskManangerSystem.Models.DataBean
{
    [Comment("员工账户信息"), Index(nameof(EmployeeAlias), IsUnique = true)]
    public class EmployeeAccount : IEmployee
    {
        [Key, Comment("GUID")]
        public Guid EmployeeId { set; get; } = Guid.NewGuid();

        [Comment("加密ID")]
        public string HashId { get; set; } = string.Empty;

        [Comment("账户名称"), Required(ErrorMessage = "账户名不可为空"), ConcurrencyCheck, RegularExpression(@"^[\da-z]{5,16}$", ErrorMessage = "员工ID必须是普通字符[数字|字母(区分大小写)]")]
        public string EmployeeAlias { get; set; } = string.Empty;

        [Comment("账户密码"), Required(ErrorMessage = "密码不可为空"), ConcurrencyCheck, StringLength(128, MinimumLength = 16, ErrorMessage = "密码长度不足")]
        public string EmployeePwd { get; set; } = string.Empty;

        [Comment("账户权限"), Required(ErrorMessage = "权限级不能为空"), ConcurrencyCheck, DefaultValue(1), Range(0, 100, ErrorMessage = "权限不能低于0")]
        public int AccountPermission { get; set; } = 1;
        // 当权限为0时，视为封存账户




        public EmployeeAccount()
        {
            SetHashId();
        }

        public EmployeeAccount(string name, string pwd, int ap = 1)
        {
            EmployeeAlias = name;
            EmployeePwd = pwd;
            AccountPermission = ap;
            SetHashId();
        }

        public bool Update(int level) { this.AccountPermission = level == 0 ? 0 : (this.AccountPermission + level); return level != 0; }

        public bool Update(string newPwd) { this.EmployeePwd = EmployeePwd != newPwd ? newPwd : EmployeePwd; return EmployeePwd != newPwd; }

        public void SetHashId() { this.HashId = this.EmployeeId.ToString().ComputeSHA512Hash(); }

    }

    [Comment("员工信息")]
    public class EmployeeInfo
    {
        [Key, Comment("员工ID")]
        public Guid EmployeeId { get; set; }

        [NotMapped]
        public string HashId => EmployeeId.ToString().ComputeSHA512Hash();

        [MinLength(2, ErrorMessage = "姓名长度必须大于等于2"), Comment("员工姓名")]
        public string? EmployeeName { get; set; }

        [MinLength(11, ErrorMessage = "手机号长度必须大于等于11"), Comment("员工联系方式")]
        public string? EmployeeContactWay { get; set; }


        [ForeignKey("EmployeeId"), Column("EmployeeId")]
        public EmployeeAccount? EmployeeSystemAccount { get; set; }

    }
}