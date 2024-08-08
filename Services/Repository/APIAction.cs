using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManangerSystem.Models;
using TaskManangerSystem.Services.Crypto;


//using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Tool;

namespace TaskManangerSystem.Services.Repository
{
    public static class DelegateExpressionTree
    {
        public readonly static Expression<Func<Category, object?>> paCategory = ex => ex.ParentCategory;

        public readonly static Expression<Func<Customer, object?>> customer = ex => ex.Categories;

        public readonly static Expression<Func<InventoryInfo, object?>> inventory = ex => ex.Categories;

        public readonly static Expression<Func<TaskAffair, object?>>[] task =
            [
            e=>e.Customers,
            e=>e.Categorys,
            e=>e.EmployeeAccounts
            ];
    }

    public class DBAction(ManagementSystemContext context)
    {
        private Category? customer;
        public async Task<int> InitAdminAccount()
        {
            //logger.LogInformation("初始化账户信息: {0}...{1}", SystemInfo.admin[0].EmployeeAlias, SystemInfo.admin[SystemInfo.admin.Length - 1].EmployeeAlias);
            
            foreach (var item in SystemInfo.admin)
                context.Entry(item).State = EntityState.Added;
            return await context.SaveChangesAsync();
        }

        public async Task<int> InitCategory()
        {
            //logger.LogInformation("初始化分类信息: {0}...{1}", SystemInfo.categories[0].CategoryName, SystemInfo.categories[SystemInfo.categories.Length - 1].CategoryName);
            foreach (var item in SystemInfo.categories)
            {
                await context.AddAsync(item);
                await context.SaveChangesAsync();
            }

            customer = await context.categories.FirstOrDefaultAsync(e => e.SortSerial == SystemInfo.CUSTOMER);
            if (customer is Category)
            {
                foreach(Category item in SystemInfo.customer)
                {
                    item.ParentCategoryId = customer.CategoryId;
                    context.Entry(item).State = EntityState.Added;
                }

            }
            return await context.SaveChangesAsync();
        }

        public async Task<int> InitCustomer()
        {
            //logger.LogInformation("初始化客户信息: {0}...{1}", SystemInfo.customer[0].CategoryName, SystemInfo.customer[SystemInfo.customer.Length - 1].CategoryName);
            CategoryRepositoryAsync CRA = new(context);

            if (!await CRA.ExistsCategoryBySerialAsync(SystemInfo.CUSTOMER))
            {
                _ = await InitCategory();
            }
            customer = await context.categories.FirstOrDefaultAsync(e => e.SortSerial == SystemInfo.CUSTOMER);
            if(customer is Category e)
            foreach (Customer item in SystemInfo.customers)
            {
                item.CustomerType = e.CategoryId;
                context.Entry(item).State = EntityState.Added;
            }
            return context.SaveChanges();
        }
    }

    public class EmployeeRepositoryAsync(ManagementSystemContext storage) : RepositoryAsync<Employee>(storage)
    {
        public async Task<bool> LoginCheckAsync(EmployeeAccountForLoginOrAdd account)
            => await ExistAsync(e => e.AccountPermission >= 1 && e.AccountPermission <= 255 && e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd);

        public async Task<bool> RigisterCheckAsync(EmployeeAccountForLoginOrAdd account)
            => !await ExistsEmployeeByNameAsync(account.EmployeeAlias) && account.EmployeePwd.Length >= 8 && account.EmployeePwd.Length <= 128;

        public async Task<bool> ExistsEmployeeAsync(Guid id) => await ExistAsync(e => e.EmployeeId == id);
        public async Task<bool> ExistsEmployeeByNameAsync(string name) => await ExistAsync(e => e.EmployeeAlias == name);
        public async Task<bool> ExistsEmployeeByHashIdAsync(string hashId) => await ExistAsync(c => c.HashId == hashId);

        public async Task<Employee?> TryGetEmployeeByNameAsync(string name) => await TryGetAsync(e => e.EmployeeAlias == name);
        public async Task<Employee?> TryGetEmployeeByHashIdAsync(string hashId) => await TryGetAsync(e => e.HashId == hashId);


        public async Task<bool> UpdatePwdAsync(Employee obj, string newPwd, string oldPwd)
        => await obj.ConditionalCheckAsync(e => e.EmployeePwd == oldPwd.ComputeSHA512Hash(),
                async c =>
                {
                    _ = obj.Update(newPwd.ComputeSHA512Hash());
                    return await UpdateAsync(obj);
                }, false);

        public async Task<bool> UpdateLevelAsync(Employee obj, int level = 1)
        {
            obj.Update(level);
            return await UpdateAsync(obj);
        }

        public async Task<bool> DisabledAsync(Employee obj)
        => await UpdateLevelAsync(obj, 0);

        public async Task<PageContent<Employee>?> SearchAsync(int page, int PageContent, bool isUp)
        => await SearchAsync(e => e.EmployeeId, page, PageContent, c => c.AccountPermission >= 1, isUp);

    }

    [Obsolete("该类不具有封装意义")]
    public class EmployeeRepository(ManagementSystemContext storage) : Repository<Employee>(storage)
    {
        public Employee? TryGetEmployeeByName(string name) => TryGet(e => e.EmployeeAlias == name);
    }


    public class CategoryRepositoryAsync(ManagementSystemContext storage) : IncludeRepositoryAsync<Category>(storage, [DelegateExpressionTree.paCategory])
    {
        #region 检查方法

        public async Task<bool> AddCheckAsync(CategoryForAddOrUpdate add) => await ExistsCategoryByNameAsync(add.CategoryName) || !await ExistsCategoryBySerialAsync(add.ParentSortSerial);
        public async Task<bool> UpdateCheckAsync(int sortSerial, CategoryForAddOrUpdate update) => await ExistsCategoryBySerialAsync(sortSerial) && sortSerial != 100 && (await ExistsCategoryBySerialAsync(update.ParentSortSerial) || update.ParentSortSerial == 0);


        public async Task<bool> ExistsCategoryAsync(Guid? id) => await ExistAsync(e => e.CategoryId == id);
        public async Task<bool> ExistsCategoryBySerialAsync(int serial) => await ExistAsync(e => e.SortSerial == serial);
        public async Task<bool> ExistsCategoryByNameAsync(string name) => await ExistAsync(e => e.CategoryName == name);

        public async Task<Category?> GetCategoryAsync(Guid? id) => await TryGetAsync(e => e.CategoryId == id);
        public async Task<Category?> GetCategoryBySerialAsync(int serial) => await TryGetAsync(e => e.SortSerial == serial);
        public async Task<Category?> GetCategoryByNameAsync(string name) => await TryGetAsync(e => e.CategoryName == name);

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
            && (await SearchAsync(keySelector: e => e.CategoryLevel, predicate: t => t.ParentCategoryId == s.CategoryId))?.PageData is List<Category> b ? b : null;


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

        /////<summary>
        ///// 低级封装 ： 根据ID 返回分类等级
        /////</summary>
        //public async Task<int> GetLevelByIdAsync(Guid? parId)
        //{
        //    var obj = await GetCategoryAsync(parId);
        //    if (obj != null)
        //        return !await ExistsCategoryAsync(obj.ParentCategoryId)
        //        ? obj!.CategoryLevel
        //        : await GetLevelByIdAsync(obj!.ParentCategoryId) + 1;
        //    else return 1;
        //}



        /// <summary>
        /// 低级封装 ： 根据序列号 返回分类等级
        /// </summary>
        /// <param name="parId">parent-Serial</param>
        /// <returns>level</returns>
        public async Task<int> GetLevelBySerialAsync(int parSerial)
            => await ExistsCategoryBySerialAsync(parSerial) && await GetCategoryBySerialAsync(parSerial) is Category b ? b.CategoryLevel : 1;


        /// <summary>
        /// 获取最后一个Serial
        /// </summary>
        /// <returns>最后一个Serial</returns>
        public async Task<int> GetLastSerialAsync()
        => (await AsEntity.OrderBy(e => e.SortSerial).LastOrDefaultAsync())?.SortSerial ?? 100;



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



        public async Task<PageContent<Category>?> GetCategoryListAsync(int page = 1, int pageSize = 100)
        {
            return await SearchAsync(e => e.SortSerial, page, pageSize, s => s.CategoryLevel >= 1);
        }


        public async Task<PageContent<Category>?> GetCategoryListByLevelAsync(int level = 1, int page = 1, int pageSize = 100)

        {
            return await SearchAsync(e => e.CategoryLevel, page, pageSize, s => s.CategoryLevel == level);
        }


        public async Task<PageContent<Category>?> GetCategoryListByParIdAsync(int parId = 100, int page = 1, int pageSize = 120)

        {
            return await ExistsCategoryBySerialAsync(parId) && await GetCategoryBySerialAsync(parId) is Category c
            ? await SearchAsync(e => e.CategoryLevel, page, pageSize, s => s.ParentCategoryId == c.CategoryId) : null;

        }
        #endregion

    }

    public class CategoryRepository(ManagementSystemContext storage) : IncludeRepository<Category>(storage, DelegateExpressionTree.paCategory)
    {
        public bool ExistsCategoryBySerial(int serial) => Exist(e => e.SortSerial == serial);
        public Category? TryGetCategoryByName(string name) => TryGet(e => e.CategoryName == name);
        public Category? TryGetCategoryBySerial(int serial) => TryGet(e => e.SortSerial == serial);
        public int GetLastSerial() => AsEntity.OrderBy(e => e.SortSerial).LastOrDefault()?.SortSerial ?? 100;
        public int GetLevelBySerial(int parSerial) => ExistsCategoryBySerial(parSerial) && TryGetCategoryBySerial(parSerial) is Category b ? b.CategoryLevel : 1;
    }

    public class CustomerRepositoryAsync(ManagementSystemContext storage) : IncludeRepositoryAsync<Customer>(storage, [DelegateExpressionTree.customer])
    {
        public async Task<bool> ExistsCustomerByNameAsync(string Hashname) => await ExistAsync(e => e.CustomerName == Hashname);

        public Customer? GetCustomerByName(string HashName) => AsEntity.Where(e => e.CustomerName == HashName).FirstOrDefault();

        public async Task<PageContent<Customer>?> SearchAsync(int pageIndex, int pageSize)
        => await SearchAsync(t => t.AddTime, pageIndex, pageSize, c => c.ClientGrade >= 1);
    }

    public class CustomerRepository(ManagementSystemContext storage) : IncludeRepository<Customer>(storage, DelegateExpressionTree.customer)
    {
        public Customer? TryGetCustomerByName(string name) => TryGet(e => e.CustomerName == name);
    }


    public class InventoryRepositoryAsync(ManagementSystemContext storage) : IncludeRepositoryAsync<InventoryInfo>(storage, [DelegateExpressionTree.inventory])
    {
        public async Task<bool> ExistsInventoryAsync(Guid guid) => await ExistAsync(e => e.ProductId == guid);
        public async Task<bool> ExistsInventoryByNameAsync(string name) => await ExistAsync(e => e.ProductName == name);

        public async Task<InventoryInfo?> GetInventoryAsync(Guid guid) => await TryGetAsync(guid);
        public async Task<InventoryInfo?> GetInventoryByNameAsync(string name) => await TryGetAsync(e => e.ProductName == name);

        public async Task<InventoryInfo?> TryGetInventoryAsync(Guid guid) => await ExistsInventoryAsync(guid) ? await TryGetAsync(guid) : null;
        public async Task<InventoryInfo?> TryGetInventoryByNameAsync(string name) => await ExistsInventoryByNameAsync(name) ? await GetInventoryByNameAsync(name) : null;
        public async Task<PageContent<InventoryInfo>?> SearchAsync(int pageIndex, int pageSize)
            => await SearchAsync(c => c.ProductName, pageIndex, pageSize);
    }

    [Obsolete("该封装过于简略")]
    public class InventoryRepository(ManagementSystemContext storage) : Repository<InventoryInfo>(storage)
    {
        public bool ExistsInventoryByName(string name) => Exist(e => e.ProductName == name);

        public InventoryInfo? TryGetInventoryByName(string name) => TryGet(e => e.ProductName == name);
    }

    public class TaskAffairRepositoryAsync(ManagementSystemContext storage) : IncludeRepositoryAsync<TaskAffair>(storage, DelegateExpressionTree.task)
    {
        public async Task<bool> ExistsTaskAffair(Guid guid) => await ExistAsync(e => e.TaskId == guid);
        public async Task<bool> ExistsTaskAffairBySerialAsync(int serial) => await ExistAsync(e => e.Serial == serial);

        public async Task<TaskAffair?> GetTaskAffairBySerial(int serial) => await TryGetAsync(e => e.Serial == serial);

        public async Task<bool> UpdateAsync(TaskAffair af, TaskAffairForUpdate afUp)
        {
            af.Update(afUp);
            return await UpdateAsync(af);
        }

        public async Task<bool> DeleteAsync(TaskAffair affair)
        {
            affair.TaskType = (await storage.categories.FirstAsync(e => e.SortSerial == 100)).CategoryId;
            return true;
        }

        public async Task<PageContent<TaskAffair>?> SearchAsync(int index, int size)
            => await SearchAsync(e => e.Serial, index, size);
    }

    public class TaskAffairRepository(ManagementSystemContext storage) : IncludeRepository<TaskAffair>(storage, DelegateExpressionTree.task)
    {
        public int GetLastSerial() => AsEntity.OrderBy(e => e.Serial).LastOrDefault()?.Serial ?? 0;
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