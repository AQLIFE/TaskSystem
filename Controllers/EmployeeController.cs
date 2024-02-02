using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.DbContextConfg;
using TaskManangerSystem.Models;
using TaskManangerSystem.Actions;

namespace TaskManangerSystem.Controllers
{

    [ApiController]
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
        public async Task<ActionResult<IEnumerable<AliasEmployeeSystemAccount>>> GetEmployeeSystemAccounts()
        {
            // return await _context.EmployeeSystemAccounts.Select(x=>x.ToAlias(default)).ToListAsync<AliasEmployeeSystemAccount>();

            var obj = _context.EmployeeSystemAccounts.ToList();
            List<AliasEmployeeSystemAccount> list = new ();
            foreach (var item in obj)
            {
                list.Add( Comon.ToAlias(item));
            }
            // return await obj.ToListAsync();
            // return await _context.EmployeeSystemAccounts.ToListAsync();
        }

        // GET: api/EmployeeSystemAccounts/md5-string
        [HttpGet("{id}")]//查看用户
        public async Task<ActionResult<AliasMd5>> GetEmployeeSystemAccount(string id)
        {
            var employeeSystemAccount = await _context.AliasMds.Where(x => x.EncryptionId == id).FirstAsync();

            if (employeeSystemAccount == null)
            {
                return NotFound();
            }

            return employeeSystemAccount;
        }

        // PUT: api/EmployeeSystemAccounts/5
        [HttpPut("{id}")]// 更新指定用户
        public async Task<IActionResult> PutEmployeeSystemAccount(string id, AliasEmployeeSystemAccount employeeSystemAccount)
        {
            if (!EmployeeSystemAccountExists(id))
            {
                Console.WriteLine(EmployeeSystemAccountExists(id));
                return BadRequest();
            }

            Guid ids = _context.AliasMds.Where(e => e.EncryptionId == id).First().Id;
            // var obj = ;
            _context.Entry<EmployeeSystemAccount>(employeeSystemAccount.ToEmployeeSystemAccount(ids)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine(employeeSystemAccount.ToEmployeeSystemAccount().EmployeeAlias);
                return NotFound("数据已被锁定，请稍后再试");
            }

            return Ok("已更新" + employeeSystemAccount.EmployeeAlias);
        }

        // POST: api/EmployeeSystemAccounts
        [HttpPost]//增加
        public async Task<ActionResult<AliasMd5>> PostEmployeeSystemAccount(AliasEmployeeSystemAccount employeeSystemAccount)
        {
            EmployeeSystemAccount part = employeeSystemAccount.ToEmployeeSystemAccount();

            _context.EmployeeSystemAccounts.Add(part);
            await _context.SaveChangesAsync();

            return await GetEmployeeSystemAccount(Comon.GetMD5(part.EmployeeId.ToString()));
            // return CreatedAtAction("GetEmployeeSystemAccount", new { id = part.EmployeeId });
        }

        // DELETE: api/EmployeeSystemAccounts/md5-string
        [HttpDelete("{id}")]//删除
        public async Task<IActionResult> DeleteEmployeeSystemAccount(string id)
        {
            Guid ida = _context.AliasMds.Where(e => e.EncryptionId == id).First().Id;
            var employeeSystemAccount = await _context.EmployeeSystemAccounts.Where(x => x.EmployeeId == ida).FirstAsync();
            if (employeeSystemAccount == null)
            {
                return NotFound();
            }

            _context.EmployeeSystemAccounts.Remove(employeeSystemAccount);
            await _context.SaveChangesAsync();

            return Ok("已删除" + id);
        }

        //查找指定用户是否存在

        private bool EmployeeSystemAccountExists(string id)
        {
            return _context.AliasMds.Any(e => e.EncryptionId == id);
        }
    }
}