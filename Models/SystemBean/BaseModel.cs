using TaskManangerSystem.IServices.BeanServices;

namespace TaskManangerSystem.Models.SystemBean
{
    // public abstract class BasePart : IPart
    // {
    //     public virtual string EmployeeAlias { get; set; } = string.Empty;
    //     public virtual string EmployeePwd { get; set; } = string.Empty;
    // }

    // public abstract class BasePartInfo : IPartInfo
    // {
    //     public virtual string EmployeeAlias { get; set; } = string.Empty;
    //     public virtual int AccountPermission { get; set; } = 0;
    // }

    public abstract class BaseEmployee : IEmployee
    {
        public BaseEmployee()
        {
            EmployeeAlias = string.Empty;
            EmployeePwd = string.Empty;
        }
        // 从 注册 转为 实体
        public BaseEmployee(IPart obj)
        {
            EmployeeId = Guid.NewGuid();
            EmployeeAlias = obj.EmployeeAlias;
            EmployeePwd = obj.EmployeePwd;
            AccountPermission = 1;
        }// 新增用户

        public BaseEmployee(IPartInfo obj, string pwd, Guid id)
        {
            EmployeeId = id;
            EmployeeAlias = obj.EmployeeAlias;
            EmployeePwd = pwd;
            AccountPermission = obj.AccountPermission;
        }// 修改用户

        public virtual Guid EmployeeId { get; set; }
        public virtual string EmployeeAlias { get; set; }
        public virtual string EmployeePwd { get; set; }
        public virtual int AccountPermission { get; set; }
    }


    //----------------------------------------------------------------

    // [Obsolete("该接口将移除，使用IEmployee")]
    // public abstract class BaseEncrypt : IEncrypt
    // {
    //     public virtual string EncryptionId { get; set; }
    //     public virtual Guid EmployeeId { get; set; }
    //     public virtual string EmployeeAlias { get; set; }
    //     public virtual string EmployeePwd { get; set; }
    //     public virtual int AccountPermission { get; set; }
    //     // public virtual IPartInfo ToBasePartInfo() => new BasePartInfo(this);

    //     public BaseEncrypt() { }
    // }

    // public abstract class BaseCateInfo : ICateInfo
    // {
    //     public virtual int SortSerial { set; get; }
    //     public virtual int ParentSortSerial { set; get; }
    //     public virtual string CategoryName { get; set; }
    //     public virtual int CategoryLevel { get; set; }
    //     public virtual string? Remark { get; set; }

    //     public BaseCateInfo() { }
    //     public BaseCateInfo(ICategory category, int parId)
    //     {
    //         SortSerial = category.SortSerial;
    //         ParentSortSerial = parId;
    //         CategoryName = category.CategoryName;
    //         CategoryLevel = category.CategoryLevel;
    //         Remark = category.Remark;
    //     }

    //     // public virtual ICategory ToCategory(Guid Id, Guid? ParId) => new BaseCategory(this, Id, ParId);
    //     public virtual ICategory ToCategory(Guid Id, Guid? ParId) => new Category(this, Id, ParId);
    // }

    // [Obsolete("废弃，使用Customer")]
    // public abstract class BaseCustomer : ICustomer
    // {
    //     public virtual Guid CustomerId { get; set; }
    //     public virtual string CustomerName { get; set; }
    //     public virtual string? CustomerContactWay { get; set; }
    //     public virtual string? CustomerAddress { get; set; }
    //     public virtual Guid CustomerType { get; set; }
    //     public virtual int ClientGrade { get; set; }
    //     public virtual DateTime AddTime { get; set; }

    //     public BaseCustomer() { }

    //     public BaseCustomer(ICustomerInfo customer, Guid cateId, int defaultLevel = 1)
    //     {
    //         CustomerId = Guid.NewGuid();
    //         CustomerName = customer.CustomerName;
    //         CustomerContactWay = customer.CustomerContactWay;
    //         CustomerAddress = customer.CustomerAddress;
    //         ClientGrade = defaultLevel;
    //         AddTime = DateTime.Now;
    //         CustomerType = cateId;
    //     }
    //     //     public ICustomerInfo ToCustomerInfo(int serial = 100)
    //     //         => new BaseCustomerInfo(this, serial);
    // }
}