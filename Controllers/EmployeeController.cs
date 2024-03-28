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
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<IEnumerable<IPartInfo>>> GetEmployeeSystemAccounts(int page = 1, int pageSize = SystemInfo.pageSize)
            => await context.employees.OrderByDescending(e => e.AccountPermission)
            .Skip((page - 1) * pageSize).Take(pageSize).Select(x => x as IPartInfo).ToListAsync();


        // POST: api/EmployeeSystemAccounts
        [HttpPost]//增加
        public async Task<string?> PostEmployeeSystemAccount(Part info)
        {
            // if (action.ExistsEmployeeByName(info.EmployeeAlias)) return "名称已存在";

            EmployeeAccount part = (EmployeeAccount)info.ToEmployee();

            context.employees.Add(part);
            await context.SaveChangesAsync();

            ShaEncrypted encrypted = part.EmployeeId.ToString();
            return GetEmployeeSystemAccount(encrypted.ComputeSHA512Hash())?.EmployeeAlias;
        }

        // GET: api/EmployeeSystemAccounts/SHA512-string
        [HttpGet("{id}")]//查看用户 公开信息
        public IPartInfo GetEmployeeSystemAccount(string id)=> action.GetEmployeeByHashId(id)!.ToPartInfo();

        // 大写
        // PUT: api/EmployeeSystemAccounts/5
        [HttpPut("{id}")]// 更新指定用户
        public async Task<string?> PutEmployeeSystemAccount(string id, PartInfo info)
        {
            // if (id == String.Empty || id == null || action.ExistsEmployee(id)) return "不存在这个账户";
            // else if (action.ExistsEmployeeByName(info.EmployeeAlias)) return "账户名重复";

            // Console.WriteLine("在方法内");
            EmployeeAccount? objs = action.GetEmployee(id);
            objs!.AccountPermission = info.AccountPermission;
            objs!.EmployeeAlias = info.EmployeeAlias;

            context.Entry<EmployeeAccount>(objs).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return info.EmployeeAlias;
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