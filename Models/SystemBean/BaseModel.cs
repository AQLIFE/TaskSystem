using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Models.SystemBean
{
    public class BasePart(BaseEmployee? employee) : IPart
    {
        public virtual string? EmployeeAlias { get; set; } = employee?.EmployeeAlias;
        public virtual string? EmployeePwd { get; set; } = employee?.EmployeePwd;

        public BasePart() : this(null) { }
        public virtual IEmployee ToEmployee() => new BaseEmployee(this);
    }

    public class BasePartInfo : IPartInfo
    {
        public virtual string EmployeeAlias { get; set; } /* = employee.EmployeeAlias; */
        public virtual int AccountPermission { get; set; }/*  = employee.AccountPermission; */

        public BasePartInfo() { }
        public BasePartInfo(IPartInfo employee)
        {
            EmployeeAlias = employee.EmployeeAlias;
            AccountPermission = employee.AccountPermission;
        }
        public virtual IEmployee ToEmployee(string pwd, Guid id) => new BaseEmployee(this, pwd, id);
    }

    public class BaseEmployee : IEmployee
    {
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
        }// 查找用户

        public virtual Guid EmployeeId { get; set; }
        public virtual string EmployeeAlias { get; set; }
        public virtual string EmployeePwd { get; set; }
        public virtual int AccountPermission { get; set; }
        public virtual IPartInfo ToBasePartInfo() => new BasePartInfo(this);
        public virtual IPart ToBasePart() => new BasePart(this);

        public BaseEmployee() { }

    }

    public class BaseEncrypt : IEncrypt
    {
        public virtual string EncryptionId { get; set; }
        public virtual Guid EmployeeId { get; set; }
        public virtual string EmployeeAlias { get; set; }
        public virtual string EmployeePwd { get; set; }
        public virtual int AccountPermission { get; set; }
        public virtual IPartInfo ToBasePartInfo() => new BasePartInfo(this);

        public BaseEncrypt() { }
    }

    public class BaseCateInfo : ICateInfo
    {
        public virtual int SortSerial { set; get; }
        public virtual int ParentSortSerial { set; get; }
        public virtual string CategoryName { get; set; }
        public virtual int CategoryLevel { get; set; }
        public virtual string? Remark { get; set; }

        public BaseCateInfo() { }
        public BaseCateInfo(ICategory category, int parId)
        {
            SortSerial = category.SortSerial;
            ParentSortSerial = parId;
            CategoryName = category.CategoryName;
            CategoryLevel = category.CategoryLevel;
            Remark = category.Remark;
        }

        // public virtual ICategory ToCategory(Guid Id, Guid? ParId) => new BaseCategory(this, Id, ParId);
        public virtual ICategory ToCategory(Guid Id, Guid? ParId) => new Category(this, Id, ParId);
    }

    public class BaseCategory/* (ICateInfo? cateInfo, Guid id, Guid? parId) */ : ICategory
    {
        public virtual Guid CategoryId { get; set; } /* = id; */
        public virtual Guid? ParentCategoryId { get; set; }/*  = parId; */
        public virtual int SortSerial { set; get; } /* = cateInfo.SortSerial; */
        public virtual string CategoryName { get; set; } /* = cateInfo.CategoryName; */
        public virtual int CategoryLevel { get; set; } /* = cateInfo.CategoryLevel; */
        public virtual string? Remark { get; set; }/*  = cateInfo.Remark; */
        public BaseCategory() /* : this(null, Guid.NewGuid(), Guid.NewGuid()) */ { }
        public BaseCategory(ICateInfo? cateInfo, Guid id, Guid? parId)
        {
            CategoryId = id;
            ParentCategoryId = parId;
            SortSerial = cateInfo.SortSerial;
            CategoryName = cateInfo.CategoryName;
            CategoryLevel = cateInfo.CategoryLevel;
            Remark = cateInfo.Remark;
        }

        public BaseCategory(CaInfo ca, Guid id, Guid? parId, int sort, int level)
        {
            CategoryId = id;
            ParentCategoryId = parId;
            SortSerial = sort;
            CategoryName = ca.CategoryName;
            CategoryLevel = level;
            Remark = ca.Remark;
        }


        // public virtual ICateInfo ToCateInfo(ManagementSystemContext context) => new BaseCateInfo(this, context.categories.Find(this.ParentCategoryId).SortSerial);
    }


    public class BaseCustomer : ICustomer
    {
        public virtual Guid CustomerId { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string? CustomerContactWay { get; set; }
        public virtual string? CustomerAddress { get; set; }
        public virtual Guid CustomerType { get; set; }
        public virtual int ClientGrade { get; set; }
        public virtual DateTime AddTime { get; set; }

        public BaseCustomer() { }

        public BaseCustomer(ICustomerInfo customer, Guid cateId, int defaultLevel = 1)
        {
            CustomerId = Guid.NewGuid();
            CustomerName = customer.CustomerName;
            CustomerContactWay = customer.CustomerContactWay;
            CustomerAddress = customer.CustomerAddress;
            ClientGrade = defaultLevel;
            AddTime = DateTime.Now;
            CustomerType = cateId;
        }
        public ICustomerInfo ToCustomerInfo(int serial = 100)
            => new BaseCustomerInfo(this, serial);
    }


    public class BaseCustomerInfo : ICustomerInfo
    {
        public virtual string CustomerName { get; set; }
        public virtual string? CustomerContactWay { get; set; }
        public virtual string? CustomerAddress { get; set; }
        public virtual int Serial { get; set; }

        public BaseCustomerInfo() { }

        public BaseCustomerInfo(ICustomer customer, int serial)
        {
            CustomerName = customer.CustomerName;
            CustomerContactWay = customer.CustomerContactWay;
            CustomerAddress = customer.CustomerAddress;
            this.Serial = serial;
        }

        public BaseCustomer ToBaseCustomer(Guid cateId)
            => new BaseCustomer(this, cateId);

        public TaskCustomer ToCustomer(Guid cateId)
            => new TaskCustomer(this, cateId);
    }

}