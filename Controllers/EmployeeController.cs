using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Services;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.IServices.BeanServices;
using System.Security.Cryptography;
using System.Text;

namespace TaskManangerSystem.Controllers
{

    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class EmployeeController(ManagementSystemContext context) : ControllerBase
    {

        /// <summary>
        /// 返回所有的账户信息
        /// - 完成数据脱敏
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "99")]
        public async Task<ActionResult<IEnumerable<AliasAccount>>> GetEmployeeSystemAccounts()
        {
            return await context.employees.Select(x => x.ToAliasAccount(default, default)).ToListAsync();
        }

        // POST: api/EmployeeSystemAccounts
        [HttpPost]//增加
        public async Task<string?> PostEmployeeSystemAccount(Part employeeSystemAccount)
        {
            if (EmployeeSystemAccountExists(employeeSystemAccount.EmployeeAlias)) return "名称已存在";
            EmployeeAccount part = employeeSystemAccount.ToEmployeeAccount();

            context.employees.Add(part);
            await context.SaveChangesAsync();

            return GetEmployeeSystemAccount(ComputeSHA256Hash(part.EmployeeId.ToString()))?.Result?.EmployeeAlias;
        }

        // GET: api/EmployeeSystemAccounts/SHA256-string
        [HttpGet("{id}")]//查看用户
        public async Task<Info?> GetEmployeeSystemAccount(string id)
        {
            EncryptAccount? encrypt = await context.encrypts.Where(x => x.EncryptionId == id).FirstOrDefaultAsync();
            return encrypt!=null ?new Info(){
                AccountPermission = encrypt.AccountPermission,
                EmployeeAlias = encrypt.EmployeeAlias
            }:null;
        }

        // 大写
        // PUT: api/EmployeeSystemAccounts/5
        [HttpPut("{id}")]// 更新指定用户
        public async Task<string?> PutEmployeeSystemAccount(string id, Info employeeSystemAccount)
        {
            EncryptAccount objs = context.encrypts.Where(e => e.EncryptionId == id).First();
            context.Entry<EmployeeAccount>(employeeSystemAccount.ToEmployeeAccount(objs.EmployeePwd,objs.EmployeeId)).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return employeeSystemAccount.EmployeeAlias;
        }

        
        // DELETE: api/EmployeeSystemAccounts/SHA256-string
        [HttpDelete("{id}")]//删除
        public async Task<string> DeleteEmployeeSystemAccount(string id)
        {
            Guid ida = context.encrypts.Where(e => e.EncryptionId == id).First().EmployeeId;
            var employeeSystemAccount = await context.employees.Where(x => x.EmployeeId == ida).FirstOrDefaultAsync();
            if (employeeSystemAccount == null)return "不存在这个账户";

            context.employees.Remove(employeeSystemAccount);
            await context.SaveChangesAsync();
            string str = $"已删除 {id}";
            return str;
        }

        //查找指定用户是否存在
        private bool EmployeeSystemAccountExists(string name)
            => context.encrypts.Any(e => e.EmployeeAlias == name);


        private string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Compute the hash value
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}