using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Models.SystemBean
{
    /// <summary>
    /// 用于返回脱敏数据
    /// </summary>
    public class Part : IPart, IEmployeeConverter
    {
        public string EmployeeAlias { get; set; } = string.Empty;
        public string EmployeePwd { get; set; } = string.Empty;
        public IEmployee ToEmployee() {this.EmployeePwd=ShaHashExtensions.ComputeSHA512Hash(EmployeePwd);return new EmployeeAccount(this);}
    }

    public class PartInfo : IPartInfo, IEmployeeConverter<PartInfo>
    {
        public string EmployeeAlias { get; set; } = string.Empty;
        public int AccountPermission { get; set; } = 0;
        public PartInfo(EmployeeAccount obj){
            AccountPermission=obj.AccountPermission;
            EmployeeAlias = obj.EmployeeAlias;
        }
        public IEmployee ToEmployee(string pwd, Guid id) => new EmployeeAccount(this, pwd, id);

        public PartInfo(){}
    }


    public class CateInfo : ICategoryInfo
    {
        public int SortSerial { set; get; }
        public int ParentSortSerial { set; get; }// 子类自定义
        public string CategoryName { get; set; } =string.Empty;
        public int CategoryLevel { get; set; }
        public string? Remark { get; set; }
        public CateInfo() { }
        public CateInfo(ICategory category,int parSerial){
            SortSerial = category.SortSerial;
            ParentSortSerial = parSerial;
            CategoryName = category.CategoryName;
            CategoryLevel = category.CategoryLevel;
            Remark = category.Remark;
        }
        public Category ToCategory(Guid Id, Guid? ParId)
            => new Category(this, Id, ParId);
    }


    public class MiniCate{
        public int ParentSortSerial { set; get; }// 子类自定义
        public string CategoryName { get; set; } =string.Empty;
        public string? Remark { get; set; }

        public MiniCate(){}

        public Category ToCategory(Guid? parId,int serial,int level)=>new (this,serial:serial,level:level,parId:parId);
    }

    public class CustomerInfo : ICustomerInfo
    {
        public virtual string CustomerName { get; set; } =string.Empty;
        public virtual string? CustomerContactWay { get; set; }
        public virtual string? CustomerAddress { get; set; }
        
        public CustomerInfo() { }

        public CustomerInfo(ICustomer customer, int serial)
        {
            CustomerName = customer.CustomerName;
            CustomerContactWay = customer.CustomerContactWay;
            CustomerAddress = customer.CustomerAddress;
        }

        public ICustomer ToCustomer(Guid cateId)
            => new Customer(this, cateId);
    }

}