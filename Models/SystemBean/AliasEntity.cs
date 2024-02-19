using System.Text.Json.Serialization;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Models.SystemBean
{
    /// <summary>
    /// 用于返回脱敏数据
    /// </summary>
    public class Part : BasePart
    {
        public override string EmployeeAlias { get => base.EmployeeAlias; set => base.EmployeeAlias = value; }
        public override string EmployeePwd { get => base.EmployeePwd; set => base.EmployeePwd = value; }
        public Part(EmployeeAccount employeeAccount) : base(employeeAccount) { }
        public Part() { }

        public override EmployeeAccount ToEmployee() => new EmployeeAccount(this);
    }

    public class PartInfo : BasePartInfo
    {
        public PartInfo(IEmployee employee) : base(employee) { }
        public override EmployeeAccount ToEmployee(string pwd, Guid id) => new EmployeeAccount(this, pwd, id);
    }


    public class CateInfo : BaseCateInfo
    {
        public override int SortSerial { set; get; }
        public override int ParentSortSerial { set; get; }
        public override string CategoryName { get; set; }
        public override int CategoryLevel { get; set; }
        public override string Remark { get; set; }
        public CateInfo() {}
        public override Category ToCategory(Guid Id, Guid? ParId)
            => new Category(this, Id, ParId);

    }

    public class CaInfo:BaseCateInfo{
        public override int ParentSortSerial { set; get; }
        public override string CategoryName { get; set; }
        public override string Remark { get; set; }

        public CaInfo(){}

        public Category ToCategory(Guid Id, Guid? ParId,int sort,int level)
        =>new Category(this, Id, ParId,sort,level);
    }

}