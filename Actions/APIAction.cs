using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Actions
{

    public class DBAction(ManagementSystemContext context)
    {
        public async Task<int> InitAdminAccount()
        {
            context.Entry<EmployeeAccount>(SystemInfo.admin).State = EntityState.Added;
            Console.WriteLine("添加角色");
            return await context.SaveChangesAsync();
        }

        public async Task<int> InitCategory()
        {
            foreach (var item in SystemInfo.categories)
                context.Entry<Category>(item).State = EntityState.Added;
            Console.WriteLine("添加分类");
            return await context.SaveChangesAsync();
        }

        public async Task<int> InitCustomer()
        {
            CategoryActions actions = new(context);
            Category category;
            if (!await actions.ExistsCategoryBySerialAsync(103))
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

    public class EmployeeActions(ManagementSystemContext storage, IMapper mapper) : Repository<EmployeeAccount>(storage)
    {
        public async Task<bool> LoginCheckAsync(EmployeeAccountForLoginOrAdd account)
            => await ExistAsync(e => e.AccountPermission >= 1 && e.AccountPermission <= 255 && e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd);

        public async Task<bool> RigisterCheckAsync(EmployeeAccountForLoginOrAdd account)
            => !await ExistsEmployeeByNameAsync(account.EmployeeAlias) && account.EmployeePwd.Length >= 8 && account.EmployeePwd.Length <= 128;

        public async Task<bool> ExistsEmployeeAsync(Guid id) => await ExistAsync(e => e.EmployeeId == id);
        public async Task<bool> ExistsEmployeeByNameAsync(string name) => await ExistAsync(e => e.EmployeeAlias == name);
        public async Task<bool> ExistsEmployeeByHashIdAsync(string hashId) => await ExistAsync(c => c.HashId == hashId);

        public async Task<EmployeeAccount?> GetEmployeeByNameAsync(string name) => await GetInfoAsync(e => e.EmployeeAlias == name);
        public async Task<EmployeeAccount?> GetEmployeeByHashIdAsync(string hashId) => await GetInfoAsync(e => e.HashId == hashId);

        public async Task<bool> UpdatePwdAsync(EmployeeAccount obj, string pwd, string oldPwd)
            => obj.EmployeePwd != pwd && obj.EmployeePwd == oldPwd
                ? await Task.Run(async () => { obj.EmployeePwd = pwd; return await UpdateInfoAsync(obj); }) : false;

        public async Task<bool> UpdateLevelAsync(EmployeeAccount obj, int level = 1)
        {
            obj.AccountPermission += level;
            return await UpdateInfoAsync(obj);
        }

        public async Task<bool> DisabledAsync(EmployeeAccount obj)
        {
            obj.AccountPermission = 0;
            return await UpdateInfoAsync(obj);
        }

        public async Task<PageContext<EmployeeAccountForSelectOrUpdate>?> SearchAsync(int page, int pageContext, bool isUp)
        {
            PageContext<EmployeeAccount>? x = await base.SearchAsync(page, pageContext, e => e.EmployeeId);

            if (x is null) return null;
            if (isUp)
            {
                var t = x.data.Where(e => e.AccountPermission >= 1).ToList();
                x.data = t;
                x.Sum = t.Count();
            }

            //List<EmployeeAccountForSelectOrUpdate> t = mapper.Map<List<EmployeeAccountForSelectOrUpdate>>(x.data);
            //PageContext<EmployeeAccountForSelectOrUpdate> b = new(x.pageIndex, x.MaxPage, x.Sum, t);


            return mapper.Map<PageContext<EmployeeAccountForSelectOrUpdate>>(x);
        }
    }

    public class CategoryActions(ManagementSystemContext storage) : IncludeRepository<Category, Category?>(storage)
    {
        #region 检查方法

        public async Task<bool> AddCheckAsync(CategoryForAddOrUpdate add) => await ExistsCategoryByNameAsync(add.CategoryName) || !await ExistsCategoryBySerialAsync(add.ParentSortSerial);
        public async Task<bool> UpdateCheckAsync(int sortSerial, CategoryForAddOrUpdate update) => await ExistsCategoryBySerialAsync(sortSerial) && await ExistsCategoryBySerialAsync(update.ParentSortSerial);


        public async Task<bool> ExistsCategoryAsync(Guid? id) => await ExistAsync(e => e.CategoryId == id);
        public async Task<bool> ExistsCategoryBySerialAsync(int serial) => await ExistAsync(e => e.SortSerial == serial);
        public async Task<bool> ExistsCategoryByNameAsync(string name) => await ExistAsync(e => e.CategoryName == name);

        public bool ExistsCategory(Guid? id) => Exist(e => e.CategoryId == id);
        public bool ExistsCategoryByName(string name) => Exist(e => e.CategoryName == name);
        public bool ExistsCategoryBySerial(int serial) => Exist(e => e.SortSerial == serial);

        public Category? GetCategory(Guid? id) => GetInfo(e => e.CategoryId == id);
        public Category? GetCategoryByName(string name) => GetInfo(e => e.CategoryName == name);
        public Category? GetCategoryBySerial(int serial) => GetInfo(e => e.SortSerial == serial);


        public int   TryGetCategorySortSerial(Guid? id) => ExistsCategory(id) ? GetCategory(id)!.SortSerial : 0;
        public Guid? TryGetCategoryIdBySortSerial(int sortSerial) => ExistsCategoryBySerial(sortSerial) ? GetCategoryBySerial(sortSerial)!.CategoryId : null;

        public Category? TryGetCategory(Guid? id) => ExistsCategory(id) ? GetCategory(id) : null;
        public Category? TryGetCategoryByName(string name) => ExistsCategoryByName(name) ? GetCategoryByName(name) : null;
        public Category? TryGetCategoryBySerial(int serial) => ExistsCategoryBySerial(serial) ? GetCategoryBySerial(serial) : null;


        public async Task<Category?> GetCategoryAsync(Guid? id) => await GetInfoAsync(e => e.CategoryId == id);
        public async Task<Category?> GetCategoryBySerialAsync(int serial) => await GetInfoAsync(e => e.SortSerial == serial, c => c.ParentCategory);
        public async Task<Category?> GetCategoryByNameAsync(string name) => await GetInfoAsync(e => e.CategoryName == name);

        public async Task<Category?> TryGetCategoryAsync(Guid id) => await ExistsCategoryAsync(id) ? await GetCategoryAsync(id) : null;
        public async Task<Category?> TryGetCategoryByNameAsync(string name) => await ExistsCategoryByNameAsync(name) ? await GetCategoryByNameAsync(name) : null;
        public async Task<Category?> TryGetCategoryBySerialAsync(int serial) => await ExistsCategoryBySerialAsync(serial) ? await GetCategoryBySerialAsync(serial) : null;

        #endregion

        #region  封装复合查询

        ///<summary>
        /// 查找具有相同父类的分类
        ///</summary>
        public async Task<List<Category>?> GetCategoryListByParentSerial(int parSerial)
        => await ExistsCategoryBySerialAsync(parSerial)
            && await GetCategoryBySerialAsync(parSerial) is Category s
            && (await SearchNotLimitAsync(e => e.CategoryLevel, t => t.ParentCategoryId == s.CategoryId))?.data is List<Category> b ? b : null;


        ///<summary>
        /// 低级封装 ： 根据序列号查对应的ID，不存在则返回空ID
        ///</summary>
        public async Task<Guid> GetIdBySerialAsync(int serial)
            => !await ExistsCategoryBySerialAsync(serial) ? Guid.Empty : (await GetCategoryBySerialAsync(serial))!.CategoryId;

        ///<summary>
        /// 低级封装 ： 根据ID查对应的序列号，不存在则返回0
        ///</summary>
        public async Task<int> GetSerialById(Guid id)
            => !await ExistsCategoryAsync(id) ? (await GetCategoryAsync(id))!.SortSerial : 0;

        ///<summary>
        /// 低级封装 ： 根据ID 返回分类等级
        ///</summary>
        public async Task<int> GetLevelByIdAsync(Guid? parId)
        {
            var obj = await GetCategoryAsync(parId);
            if (obj != null)
                return !await ExistsCategoryAsync(obj.ParentCategoryId)
                ? obj!.CategoryLevel
                : await GetLevelByIdAsync(obj!.ParentCategoryId) + 1;
            else return 1;
        }

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
        /// 低级封装 ： 根据序列号 返回分类等级
        /// </summary>
        /// <param name="parId">parent-Serial</param>
        /// <returns>level</returns>
        public async Task<int> GetLevelBySerialAsync(int parSerial)
            => !await ExistsCategoryBySerialAsync(parSerial) && await GetCategoryBySerialAsync(parSerial) is Category b
                ? await GetLevelByIdAsync(b.ParentCategoryId) + 1
                : 1;

        public int GetLevelBySerial(int parSerial)
            => !ExistsCategoryBySerial(parSerial) && GetCategoryBySerial(parSerial) is Category b
                ? GetLevelById(b.ParentCategoryId) + 1
                : 1;

        /// <summary>
        /// 获取最后一个Serial
        /// </summary>
        /// <returns>最后一个Serial</returns>
        public async Task<int> GetLastSerialAsync()
        => (await AsEntity.OrderBy(e => e.SortSerial).LastOrDefaultAsync())?.SortSerial ?? 100;

        public int GetLastSerial()
        => AsEntity.OrderBy(e => e.SortSerial).LastOrDefault()?.SortSerial ?? 100;

        /// <summary>
        /// 获得父分类的序列号
        /// </summary>
        /// <param name="serial">子序列号</param>
        /// <returns></returns>
        public async Task<int> GetParSerialBySerial(int serial)
        => await GetCategoryBySerialAsync(serial) is Category c && c.ParentCategoryId is Guid g &&
            await GetCategoryAsync(g) is Category cg ? cg.SortSerial : 0;

        #endregion

        #region  api专用

        public async Task<PageContext<Category>?> GetCategoryListAsync(int page = 1, int pageSize = 100)
        {
            return await IncludeSearchAsync(page, pageSize, attribute: c => c.ParentCategory, e => e.SortSerial, s => s.CategoryLevel >= 1);
        }


        public async Task<PageContext<Category>?> GetCategoryListByLevelAsync(int level = 1, int page = 1, int pageSize = 100)

        {
            return await IncludeSearchAsync(page, pageSize, c => c.ParentCategory, e => e.CategoryLevel, s => s.CategoryLevel == level);
        }


        public async Task<PageContext<Category>?> GetCategoryListByParIdAsync(int parId = 100, int page = 1, int pageSize = 120)

        {
            return await ExistsCategoryBySerialAsync(parId) && await GetCategoryBySerialAsync(parId) is Category c
            ? await IncludeSearchAsync(page, pageSize, c => c.ParentCategory, e => e.CategoryLevel, s => s.ParentCategoryId == c.CategoryId) : null;

        }
        #endregion

    }

    public class CustomerActions(ManagementSystemContext storage) : IncludeRepository<Customer, Category?>(storage)
    {
        public bool ExistsCustomerByName(string Hashname) => Exist(e => e.CustomerName == Hashname);
        public async Task<bool> ExistsCustomerByNameAsync(string Hashname) => await ExistAsync(e => e.CustomerName == Hashname);
        public Customer? GetCustomerByName(string HashName) => AsEntity.Where(e => e.CustomerName == HashName).FirstOrDefault();
        // public ICustomerInfo? GetCustomerInfoByName(string name)=>context.customers.Where(e=>e.CustomerName==name).Select(e=>e.ToCustomerInfo(e.CustomerType));

        public async Task<PageContext<Customer>?> SearchAsync(int pageIndex, int pageSize)
        => await base.IncludeSearchAsync(pageIndex, pageSize, e => e.Categories, t => t.AddTime, c => c.ClientGrade >= 1);


    }


    public class InventoryActions(ManagementSystemContext storage) : IncludeRepository<InventoryInfo, Category?>(storage)
    {
        #region 异步
        public async Task<bool> ExistsInventoryAsync(Guid guid) => await ExistAsync(e => e.ProductId == guid);
        public async Task<bool> ExistsInventoryByNameAsync(string name) => await ExistAsync(e => e.ProductName == name);

        public async Task<InventoryInfo?> GetInventoryAsync(Guid guid) => await GetInfoAsync(guid);
        public async Task<InventoryInfo?> GetInventoryByNameAsync(string name) => await GetInfoAsync(e => e.ProductName == name,e=>e.Categories);

        public async Task<InventoryInfo?> TryGetInventoryAsync(Guid guid) => await ExistsInventoryAsync(guid) ? await GetInfoAsync(guid) : null;
        public async Task<InventoryInfo?> TryGetInventoryByNameAsync(string name) => await ExistsInventoryByNameAsync(name) ? await GetInventoryByNameAsync(name) : null;


        public async Task<bool> UpdateInfoAsync(InventoryInfo x,InventoryForView y)
        {
            return await base.UpdateInfoAsync(x);
        }
        public async Task<PageContext<InventoryInfo>?> SearchAsync(int pageIndex, int pageSize)
            => await base.IncludeSearchAsync(pageIndex, pageSize, e => e.Categories, c => c.ProductName);

        #endregion

        //
        #region 同步
        public bool ExistsInventory(Guid guid) => Exist(e => e.ProductId == guid);
        public bool ExistsInventoryByName(string name) => Exist(e => e.ProductName == name);

        public InventoryInfo? GetInventory(Guid guid) => GetInfo(guid);
        public InventoryInfo? GetInventoryByName(string name) => GetInfo(e => e.ProductName == name);

        public InventoryInfo? TryGetInventory(Guid guid) => ExistsInventory(guid) ? GetInfo(guid) : null;
        public InventoryInfo? TryGetInventoryByName(string name) => ExistsInventoryByName(name) ? GetInventoryByName(name) : null;
        #endregion
    }

    public class ErrorActions : IAPIRequest<string>
    {
        private readonly string ErrorRouteMSG = "错误的请求";
        private readonly bool ErrorRouteStatus = false;
        public async Task<string?> Get() => await Task.Run(() => ErrorRouteMSG);
        public async Task<string?> Fetch() => await Task.Run(() => ErrorRouteMSG);
        public async Task<bool> Put() => await Task.Run(() => ErrorRouteStatus);
        public async Task<bool> Delete() => await Task.Run(() => ErrorRouteStatus);
        public async Task<bool> Post() => await Task.Run(() => ErrorRouteStatus);

    }

    public interface IAPIRequest<T>
    {
        Task<T?> Get();// Get all
        Task<T?> Fetch();// GET info
        Task<bool> Post();// Add Info
        Task<bool> Put();// Update Info
        Task<bool> Delete();// Remove info
    }
}