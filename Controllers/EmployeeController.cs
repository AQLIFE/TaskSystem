using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Services;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices.BeanServices;

namespace TaskManangerSystem.Controllers
{

    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class EmployeeController(ManagementSystemContext context) : ControllerBase
    {
        private EmployeeActions action = new(context);
        /// <summary>
        /// 返回所有的账户信息
        /// - 完成数据脱敏
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "99")]
        public async Task<ActionResult<IEnumerable<BasePartInfo>>> GetEmployeeSystemAccounts(int page=1,int pageSize=120)
            => await context.employees.OrderByDescending(e=>e.AccountPermission).Skip((page - 1) * pageSize).Take(pageSize).Select(x => x.ToBasePartInfo()).ToListAsync();
        

        // POST: api/EmployeeSystemAccounts
        [HttpPost]//增加
        public async Task<string?> PostEmployeeSystemAccount(Part employeeSystemAccount)
        {
            if (action.ExistsEncrypts(employeeSystemAccount.EmployeeAlias)) return "名称已存在";

            EmployeeAccount part = employeeSystemAccount.ToEmployee();

            context.employees.Add(part);
            await context.SaveChangesAsync();

            return GetEmployeeSystemAccount(GlobalActions.ComputeSHA256Hash(part.EmployeeId.ToString()))?.EmployeeAlias;
        }

        // GET: api/EmployeeSystemAccounts/SHA256-string
        [HttpGet("{id}")]//查看用户 公开信息
        public  PartInfo? GetEmployeeSystemAccount(string id)
        =>  action.GetEncrypts(id)?.ToPartInfo();

        // 大写
        // PUT: api/EmployeeSystemAccounts/5
        [HttpPut("{id}")]// 更新指定用户
        public async Task<string?> PutEmployeeSystemAccount(string id, PartInfo employeeSystemAccount)
        {
            EmployeeAccount? objs = action.GetEmployee(id);
            context.Entry<EmployeeAccount>(employeeSystemAccount.ToEmployee(objs!.EmployeePwd, objs.EmployeeId)).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return employeeSystemAccount.EmployeeAlias;
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