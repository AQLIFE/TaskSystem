using TaskManangerSystem.Models;

namespace TaskManangerSystem.IServer
{
    interface IEmployee
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeAlias { get; set; }
        public string EmployeePwd { get; set; }
        public int AccountPermission { get; set; }
    }

    interface IAlias
    {
        public string EmployeeAlias { get; set; }
        public string EmployeePwd { get; set; }
        public int AccountPermission { get; set; }
    }


    /// <summary>
    /// 脱敏数据转换抽象类
    /// </summary>
    public abstract class AEmployeeToAlias
    {

        public abstract AliasEmployeeSystemAccount ToAlias();
        public abstract AliasEmployeeSystemAccount ToAlias(char cr);
    }

    // abstract class AliasEmployeeToAlias :IAlias {
    //     public string EmployeeAlias{get;set;}
    //     public string EmployeePwd{get;set;}
    //     public int AccountPermission{get;set;}
    //     public abstract AliasEmployeeSystemAccount ToAlias();
    // }

}