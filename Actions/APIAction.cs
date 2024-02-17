using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Actions
{
    public class EmployeeActions(ManagementSystemContext context)
    {
        public bool ExistsEncrypts(string name) => context.encrypts.Any(e => e.EmployeeAlias == name);
        public bool ExistsEncrypts(Guid id) => context.encrypts.Any(e => e.EmployeeId == id);


        /// <summary>
        /// 查询对应加密ID 的原始信息
        /// </summary>
        /// <param name="id">加密ID</param>
        /// <returns>原始信息</returns>
        public EmployeeAccount? GetEmployee(string id) => context.employees.Find(GetEncrypts(id)?.EmployeeId.ToString());
        public EmployeeAccount? GetEmployeeByName(string name) => context.employees.Find(GetEncryptsByName(name)?.EmployeeId.ToString());
        // public async Task<EmployeeAccount?> GetEmployeeAsync(string id) => await context.employees.FindAsync(GetEncryptsAsync(id)?.Result?.EmployeeId);

        /// <summary>
        /// 查询加密信息
        /// </summary>
        /// <param name="id">加密ID</param>
        /// <returns>加密信息</returns>
        public EncryptAccount? GetEncrypts(string id) => context.encrypts.Where(e => e.EncryptionId == id).FirstOrDefault();
        // public async Task<EncryptAccount?> GetEncryptsAsync(string id) => await context.encrypts.Where(e => e.EncryptionId == id).FirstOrDefaultAsync();
        public EncryptAccount? GetEncryptsByName(string name) => context.encrypts.Where(e => e.EmployeeAlias == name).FirstOrDefault();

        public bool LoginCheck(Part account)
       => context.encrypts.Any(e => e.EmployeeAlias == account.EmployeeAlias && account.EmployeePwd == e.EmployeePwd);
    }

    public class CategoryActions(ManagementSystemContext context)
    {

        public bool ExistsCategoryBySerial(int serial) => context.categories.Any(e => e.SortSerial == serial);

        public Category? GetCategoryBySerial(int serial) => ExistsCategoryBySerial(serial) ? context.categories.Where(e => e.SortSerial == serial).FirstOrDefault() : null;

        public List<Category>? GetCategoryListByParentSerial(int parSerial) => ExistsCategoryBySerial(parSerial) ? context.categories.Where(e => e.ParentCategoryId == GetCategoryBySerial(parSerial)!.CategoryId).ToList() : null;

        public bool ExistsCategoryByName(string name) => context.categories.Any(e => e.CategoryName == name);
        public Category? GetCategoryByName(string name) => ExistsCategoryByName(name) ? context.categories.Where(e => e.CategoryName == name).FirstOrDefault() : null;




        // public bool ExistsCategory(Guid id) => context.categories.Any(e => e.CategoryId == id);
        public async Task<bool> ExistsCategory(Guid id){
            Console.WriteLine(id);
            return await context.categories.AnyAsync(e => e.CategoryId == id);}

        // public Category? GetCategory(Guid id) =>context.categories.Find(id);
        public async Task<Category?> GetCategory(Guid id){
            Console.WriteLine(id);return await context.categories.FindAsync(id);}

        public Guid? GetParentId(int id) => id == 0 && !ExistsCategoryBySerial(id)? null : GetCategoryBySerial(id)!.CategoryId;
        public async Task<int> GetParentSort(Guid id)
        {
            Console.WriteLine("id= "+id);
            return await ExistsCategory(id)? GetCategory(id).Result!.SortSerial : 0;}

        public string ValidateMessage = "";
        public bool Validate(CateInfo obj)
        {
            if (ExistsCategoryBySerial(obj.SortSerial)) { ValidateMessage = "已存在相同序列号"; return false; }
            if (obj.ParentSortSerial != 0 && !ExistsCategoryBySerial(obj.ParentSortSerial)) { ValidateMessage = "父分类序列号不存在"; return false; }
            if (obj.CategoryName != null && ExistsCategoryByName(obj.CategoryName)) { ValidateMessage = "分类名称已存在"; return false; }
            return true;
        }

    }
}