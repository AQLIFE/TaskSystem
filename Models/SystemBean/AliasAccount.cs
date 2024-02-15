using System.Text.Json.Serialization;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Models.SystemBean
{
    /// <summary>
    /// 用于返回脱敏数据
    /// </summary>
    public class Part :BasePart
    {
        // [JsonInclude]
        public override string EmployeeAlias { get => base.EmployeeAlias; set => base.EmployeeAlias = value; }
        // [JsonInclude]
        public override string EmployeePwd { get => base.EmployeePwd; set => base.EmployeePwd = value; }
        public Part(EmployeeAccount employeeAccount):base(employeeAccount){}
        public Part(){}

        public override EmployeeAccount ToEmployee()=>new EmployeeAccount(this);
    }

    public class PartInfo:BasePartInfo{
        public PartInfo(IEmployee employee):base( employee){}
        public override EmployeeAccount ToEmployee(string pwd,Guid id)=>new EmployeeAccount(this,pwd,id); 
    }
}