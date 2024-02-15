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
        // public EmployeeAccount GetEmployeeByEncrypts()
    }
}