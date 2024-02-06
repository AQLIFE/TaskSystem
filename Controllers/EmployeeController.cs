using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.DbContextConfg;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Controllers
{

    [ApiController,Authorize]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ManagementSystemContext _context;

        public EmployeeController(ManagementSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 返回所有的账户信息
        /// - 完成数据脱敏
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AliasAccount>>> GetEmployeeSystemAccounts()
        {
            return await _context.employees.Select(x => x.ToAliasAccount(default, default)).ToListAsync();

            // var obj = await _context.employees.ToListAsync();
            // return obj.Select(e=>e.ToAliasAccount()).ToList();
        }

        // GET: api/EmployeeSystemAccounts/md5-string
        [HttpGet("{id}")]//查看用户
        public async Task<ActionResult<EncryptAccount>> GetEmployeeSystemAccount(string id)
        {
            return await _context.encrypts.Where(x => x.EncryptionId == id).FirstAsync();
        }

        // 大写
        // PUT: api/EmployeeSystemAccounts/5
        [HttpPut("{id}")]// 更新指定用户
        public async Task<IActionResult> PutEmployeeSystemAccount(string id, AliasAccount employeeSystemAccount)
        {
            if (!EmployeeSystemAccountExists(id))
            {
                return BadRequest();
            }

            Guid ids = _context.encrypts.Where(e => e.EncryptionId == id).First().EmployeeId;
            // var obj = ;
            _context.Entry<EmployeeAccount>(employeeSystemAccount.ToEmployeeAccount(ids)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("数据已被锁定，请稍后再试");
            }

            return Ok("已更新" + employeeSystemAccount.EmployeeAlias);
        }

        // POST: api/EmployeeSystemAccounts
        [HttpPost]//增加
        public async Task<ActionResult<EncryptAccount>> PostEmployeeSystemAccount(AliasAccount employeeSystemAccount)
        {
            EmployeeAccount part = employeeSystemAccount.ToEmployeeAccount();

            _context.employees.Add(part);
            await _context.SaveChangesAsync();

            return await GetEmployeeSystemAccount(part.EmployeeAlias);
            // return CreatedAtAction("GetEmployeeSystemAccount", new { id = part.EmployeeId });
        }

        // DELETE: api/EmployeeSystemAccounts/md5-string
        [HttpDelete("{id}")]//删除
        public async Task<IActionResult> DeleteEmployeeSystemAccount(string id)
        {
            Guid ida = _context.encrypts.Where(e => e.EncryptionId == id).First().EmployeeId;
            var employeeSystemAccount = await _context.employees.Where(x => x.EmployeeId == ida).FirstAsync();
            if (employeeSystemAccount == null)
            {
                return NotFound();
            }

            _context.employees.Remove(employeeSystemAccount);
            await _context.SaveChangesAsync();

            return Ok("已删除" + id);
        }

        //查找指定用户是否存在

        private bool EmployeeSystemAccountExists(string id)
            => _context.encrypts.Any(e => e.EncryptionId == id);
    }
}