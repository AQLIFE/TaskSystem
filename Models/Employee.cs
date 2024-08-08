using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManangerSystem.IServices;
using TaskManangerSystem.Services.Crypto;

namespace TaskManangerSystem.Models
{
    [Comment("员工账户信息"), Index(nameof(EmployeeAlias), IsUnique = true)]
    public class Employee : IEmployee
    {
        [Key, Comment("GUID")]
        public Guid EmployeeId { set; get; } = Guid.NewGuid();

        [Comment("加密ID")]
        public string HashId { get; set; } = string.Empty;

        [Comment("账户名称")]
        public string EmployeeAlias { get; set; } = string.Empty;

        [Comment("账户密码")]
        public string EmployeePwd { get; set; } = string.Empty;

        [Comment("账户权限")]
        public int AccountPermission { get; set; } = 1;
        // 当权限为0时，视为封存账户




        public Employee()
        {
            SetHashId();
        }

        public Employee(string name, string pwd, int ap = 1)
        {
            EmployeeAlias = name;
            EmployeePwd = pwd;
            AccountPermission = ap;
            SetHashId();
        }

        public bool Update(int level) { AccountPermission = level == 0 ? 0 : AccountPermission + level; return level != 0; }

        public bool Update(string newPwd) { EmployeePwd = EmployeePwd != newPwd ? newPwd : EmployeePwd; return EmployeePwd != newPwd; }

        private void SetHashId() { HashId = EmployeeId.ToString().ComputeSHA512Hash(); }

    }

    [Comment("员工信息")]
    public class EmployeeInfo
    {
        [Key, Comment("员工ID")]
        public Guid EmployeeId { get; set; }

        [NotMapped]
        public string HashId => EmployeeId.ToString().ComputeSHA512Hash();

        [Comment("员工姓名")]
        public string? EmployeeName { get; set; }

        [Comment("员工联系方式")]
        public string? EmployeeContactWay { get; set; }


        [ForeignKey("EmployeeId"), Column("EmployeeId")]
        public Employee? EmployeeSystemAccount { get; set; }

    }
}