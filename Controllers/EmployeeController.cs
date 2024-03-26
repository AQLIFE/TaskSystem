using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Services;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices.BeanServices;
using System.Security.Claims;

namespace TaskManangerSystem.Controllers
{

    [ApiController,Authorize]
    [Route("api/[controller]")]
    public class EmployeeController(ManagementSystemContext context) : ControllerBase
    {
        [HttpGet("test")]
        public string test()=>"hello";
        private EmployeeActions action = new(context);
        /// <summary>
        /// 返回所有的账户信息
        /// - 完成数据脱敏
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles ="99")]
        public async Task<ActionResult<IEnumerable<IPartInfo>>> GetEmployeeSystemAccounts(int page = 1, int pageSize = 120)
            => await context.employees.OrderByDescending(e => e.AccountPermission)
            .Skip((page - 1) * pageSize).Take(pageSize).Select(x => x as IPartInfo).ToListAsync();


        // POST: api/EmployeeSystemAccounts
        [HttpPost]//增加
        public async Task<string?> PostEmployeeSystemAccount(Part employeeSystemAccount)
        {
            if (action.ExistsEmployeeByName(employeeSystemAccount.EmployeeAlias)) return "名称已存在";

            EmployeeAccount part = (EmployeeAccount)employeeSystemAccount.ToEmployee();

            context.employees.Add(part);
            await context.SaveChangesAsync();

            ShaEncrypted encrypted = part.EmployeeId.ToString();
            return GetEmployeeSystemAccount(encrypted.ComputeSHA512Hash())?.EmployeeAlias;
        }

        // GET: api/EmployeeSystemAccounts/SHA512-string
        [HttpGet("{id}")]//查看用户 公开信息
        public IPartInfo? GetEmployeeSystemAccount(string id)=> action.GetEmployee(id);

        // 大写
        // PUT: api/EmployeeSystemAccounts/5
        [HttpPut("{id}")]// 更新指定用户
        public async Task<string?> PutEmployeeSystemAccount(string id, PartInfo employeeSystemAccount)
        {
            if (id == String.Empty || id == null || action.ExistsEmployee(id)) return "不存在这个账户";
            else if (action.ExistsEmployeeByName(employeeSystemAccount.EmployeeAlias)) return "账户名重复";

            // Console.WriteLine("在方法内");
            EmployeeAccount? objs = action.GetEmployee(id);
            objs!.AccountPermission = employeeSystemAccount.AccountPermission;
            objs!.EmployeeAlias = employeeSystemAccount.EmployeeAlias;

            context.Entry<EmployeeAccount>(objs).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return employeeSystemAccount.EmployeeAlias;
        }


        [HttpPut("{id}/pwd")]
        public async Task<string> PutEmployeeSystemAccountPwd(string id, string pwd)
        {
            EmployeeAccount? employeeAccount = action.GetEmployee(id);
            if (employeeAccount == null) return "不存在这个账户";
            employeeAccount.EmployeePwd = pwd;
            context.Entry<EmployeeAccount>(employeeAccount).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return $"已更新 {id}";
        }


        // DELETE: api/EmployeeSystemAccounts/SHA256-string 
        [HttpDelete("{id}")]//删除
        public async Task<string> DeleteEmployeeSystemAccount(string id)
        {
            EmployeeAccount? employeeAccount = action.GetEmployee(id);
            if (employeeAccount == null) return "不存在这个账户";

            context.employees.Remove(employeeAccount);
            await context.SaveChangesAsync();
            return $"已删除 {id}";
        }
    }
}