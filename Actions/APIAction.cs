using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Actions
{

    public class DBAction(ManagementSystemContext context)
    {
        public int AddAdminAccount()
        {
            context.Entry<EmployeeAccount>(SystemInfo.admin).State = EntityState.Added;
            Console.WriteLine("添加角色");
            return context.SaveChanges();
        }

        public int AddCategory()
        {
            foreach (var item in SystemInfo.categories)
                context.Entry<Category>(item).State = EntityState.Added;
            Console.WriteLine("添加分类");
            return context.SaveChanges();
        }

        public int AddCustomer()
        {
            CategoryActions actions = new(context);
            Category category;
            if (!actions.ExistsCategoryBySerial(103))
            {
                category = new("本公司", 103, "管理员所属公司", 2, actions.GetCategoryBySerial(101)?.CategoryId);
                context.Entry<Category>(category).State = EntityState.Added;
                context.SaveChanges();
            }
            else category = actions.GetCategoryBySerial(103)!;

            SystemInfo.customers.CustomerType = category.CategoryId;

            context.Entry<Customer>(SystemInfo.customers).State = EntityState.Added;
            Console.WriteLine("添加客户");
            return context.SaveChanges();
        }
    }

    public class EmployeeActions(ManagementSystemContext context)
    {
        public bool ExistsEmployeeByName(string name) => context.employees.Any(e => e.EmployeeAlias == name);
        public bool ExistsEmployee(string id) => context.employees.Any(e => e.EmployeeId.ToString() == id);

        public bool ExistsEmployeeByHashId(string hashId) =>
            context.employees.ToList().Any(c => ShaHashExtensions.ComputeSHA512Hash(c.EmployeeId.ToString()) == hashId);


        /// <summary>
        /// 查询对应加密ID 的原始信息
        /// </summary>
        /// <param name="id">SHA512 加密ID</param>
        /// <returns>原始信息</returns>
        public EmployeeAccount? GetEmployee(string id) => context.employees.Where(e => e.EmployeeId.ToString() == id).FirstOrDefault();
        public EmployeeAccount? GetEmployeeByHashId(string hashId) => context.employees.ToList().Where(e => ShaHashExtensions.ComputeSHA512Hash(e.EmployeeId.ToString()) == hashId).FirstOrDefault();

        public EmployeeAccount? GetEmployeeByName(string name) => context.employees.Where(e => e.EmployeeAlias == name).FirstOrDefault();

        public bool LoginCheck(Part account)
       => context.employees.Any(e => e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd && e.AccountPermission >= 1 && e.AccountPermission <= 255);
    }

    public class CategoryActions(ManagementSystemContext context)
    {

        #region 检查方法
        private bool PrivateExistsCategoryBySerial(int serial) => context.categories.Any(e => e.SortSerial == serial);
        public bool ExistsCategoryBySerial(int serial) => serial <= 0 ? false : PrivateExistsCategoryBySerial(serial);

        private bool PrivateExistsCategoryByName(string name) => context.categories.Any(e => e.CategoryName == name);
        public bool ExistsCategoryByName(string? name) => name != null && name != string.Empty ? PrivateExistsCategoryByName(name) : false;

        private bool PrivateExistsCategory(Guid id) => context.categories.Any(e => e.CategoryId == id);
        public bool ExistsCategory(Guid? id) => id != null && id != Guid.Empty ? PrivateExistsCategory((Guid)id) : false;
        #endregion

        #region 常用查询方法

        /// <summary>
        /// Serial => Category
        /// </summary>
        /// <param name="serial">序列号</param>
        /// <returns>完整信息</returns>
        private Category? PrivateGetCategoryBySerial(int serial) => context.categories.Where(e => e.SortSerial == serial).FirstOrDefault();
        public Category? GetCategoryBySerial(int serial) => ExistsCategoryBySerial(serial) ? PrivateGetCategoryBySerial(serial) : null;
        /// <summary>
        ///  name => Category
        /// </summary>
        /// <param name="name">分类名称</param>
        /// <returns>完整信息</returns>
        private Category? PrivateGetCategoryByName(string name) => context.categories.Where(e => e.CategoryName == name).FirstOrDefault();
        public Category? GetCategoryByName(string name) => ExistsCategoryByName(name) ? PrivateGetCategoryByName(name) : null;


        /// <summary>
        /// Guid => Catrgory
        /// </summary>
        /// <param name="id">分类ID</param>
        /// <returns>完整信息</returns>
        private Category? PrivateGetCategory(Guid id) => context.categories.Find(id);
        public Category? GetCategory(Guid? id) => ExistsCategory(id) ? PrivateGetCategory((Guid)id!) : null;

        #endregion

        #region  封装复合查询
        public List<Category>? GetCategoryListByParentSerial(int parSerial) => ExistsCategoryBySerial(parSerial) ? context.categories.Where(e => e.ParentCategoryId == GetCategoryBySerial(parSerial)!.CategoryId).ToList() : null;

        /// <summary>
        /// 根据序列号获取父分类ID
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public Guid GetIdBySerial(int serial)
            => !ExistsCategoryBySerial(serial) ? Guid.NewGuid() : GetCategoryBySerial(serial)!.CategoryId;

        /// <summary>
        /// Guid => Catrgory.SortSerial
        /// </summary>
        /// <param name="id">建议填入父分类ID</param>
        /// <returns>分类ID的序列号</returns>
        public int GetSerialById(Guid? id)
            => ExistsCategory(id) ? GetCategory(id)!.SortSerial : 0;


        public int GetLevelById(Guid? parId)
        {
            var obj = GetCategory(parId);
            if (obj != null)
                return !ExistsCategory(obj.ParentCategoryId)
                ? obj!.CategoryLevel
                : GetLevelById(obj!.ParentCategoryId) + 1;
            else return 1;
        }

        /// <summary>
        /// 生成对应的level
        /// </summary>
        /// <param name="parId">parent-Serial</param>
        /// <returns>level</returns>
        public int GetLevelBySerial(int parSerial)
            => ExistsCategoryBySerial(parSerial)
                ? GetLevelById(GetCategoryBySerial(parSerial)!.ParentCategoryId) + 1
                : 1;

        /// <summary>
        /// 获取最后一个Serial
        /// </summary>
        /// <returns>最后一个Serial</returns>
        public int GetLastSerial() => context.categories.OrderBy(e => e.SortSerial).LastOrDefault()?.SortSerial ?? 100;
        // <summary>
        /// 获得父分类的序列号
        /// </summary>
        /// <param name="serial">子序列号</param>
        /// <returns></returns>
        public int GetParSerialBySerial(int serial)
        {
            var os = GetCategoryBySerial(serial)?.ParentCategoryId;
            return os is not null ? GetCategory(os)!.SortSerial : 0;
        }

        #endregion

        #region  api专用
        // public string ValidateMessage = "";
        // public bool Validate(MiniCate obj)
        // {
        //     if (obj.ParentSortSerial == 0 || !ExistsCategoryBySerial(obj.ParentSortSerial)) { ValidateMessage = "父分类序列号不存在"; return false; }
        //     if (ExistsCategoryByName(obj.CategoryName)) { ValidateMessage = "分类名称已存在"; return false; }
        //     return true;
        // }

        public List<Category> GetCategoryList(int page = 1, int pageSize = 120)
        => context.categories.OrderBy(c => c.CategoryLevel).OrderBy(c => c.SortSerial)
        .Skip((page - 1) * pageSize).Take(pageSize)
        .ToList();

        public List<Category> GetCategoryListByLevel(int level = 1, int page = 1, int pageSize = 120)
        => context.categories
            .Where(e => e.CategoryLevel == level)
            .OrderBy(c => c.SortSerial)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToList();


        public List<Category> GetCategoryListByParId(int parId = 100, int page = 1, int pageSize = 120)
        => context.categories
            .Where(e => e.ParentCategoryId == GetCategoryBySerial(parId)!.CategoryId)
            .OrderBy(c => c.SortSerial)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToList();
        #endregion

    }

    public class CustomerActions(ManagementSystemContext context)
    {
        public bool ExistsCustomerByName(string name) => context.customers.Any(e => e.CustomerName == name);
        public Customer? GetCustomerByName(string name) => context.customers.Where(e => e.CustomerName == name).FirstOrDefault();
        // public ICustomerInfo? GetCustomerInfoByName(string name)=>context.customers.Where(e=>e.CustomerName==name).Select(e=>e.ToCustomerInfo(e.CustomerType));
    }
}