using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

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
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<IEnumerable<IPartInfo>>> GetEmployeeSystemAccounts(int page = 1, int pageSize = SystemInfo.pageSize)
            => await context.employees.OrderByDescending(e => e.AccountPermission)
            .Skip((page - 1) * pageSize).Take(pageSize).Select(x => x as IPartInfo).ToListAsync();


        // POST: api/EmployeeSystemAccounts
        [HttpPost, AllowAnonymous]//增加
        public async Task<string?> PostEmployeeSystemAccount(Part info)
        {
            EmployeeAccount part = await Task.Run(() => (EmployeeAccount)info.ToEmployee());

            context.employees.Add(part);
            await context.SaveChangesAsync();

            return action.GetEmployee(part.EmployeeId.ToString())!.EmployeeAlias;
        }

        // GET: api/EmployeeSystemAccounts/SHA512-string
        [HttpGet("{id}")]//查看用户 公开信息
        public IPartInfo GetEmployeeSystemAccount(string id) => action.GetEmployeeByHashId(id)!.ToPartInfo();

        // 大写
        // PUT: api/EmployeeSystemAccounts/5
        [HttpPut("{id}")]// 更新指定用户
        public async Task<string?> PutEmployeeSystemAccount(string id, PartInfo info)
        {
            EmployeeAccount? objs = await Task.Run(() => action.GetEmployeeByHashId(id));
            objs!.AccountPermission = info.AccountPermission;
            objs!.EmployeeAlias = info.EmployeeAlias;

            context.Entry<EmployeeAccount>(objs).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return info.EmployeeAlias;
        }

        // DELETE: api/EmployeeSystemAccounts/SHA512-string 
        [HttpDelete("{id}")]//删除
        public async Task<string> DeleteEmployeeSystemAccount(string id)
        {
            EmployeeAccount? employeeAccount = await Task.Run(() => action.GetEmployee(id));
            if (employeeAccount == null) return "不存在这个账户";

            // context.employees.Remove(employeeAccount);
            // await context.SaveChangesAsync();
            employeeAccount.AccountPermission = 0;
            return employeeAccount.EmployeeAlias;
        }
    }
}