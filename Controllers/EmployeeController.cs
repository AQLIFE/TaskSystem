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

            var obj = await _context.employees.ToListAsync();
            return Comon.ToListAlias<EmployeeSystemAccount>(obj,default);
        }

        // GET: api/EmployeeSystemAccounts/md5-string
        [HttpGet("{id}")]//查看用户
        public async Task<ActionResult<EncryptEmployeeSystemAccount>> GetEmployeeSystemAccount(string id)
        {
            return await _context.encrypts.Where(x => x.EncryptionId == id).FirstAsync();
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

            Guid ids = _context.encrypts.Where(e => e.EncryptionId == id).First().Id;
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
        public async Task<ActionResult<EncryptEmployeeSystemAccount>> PostEmployeeSystemAccount(AliasEmployeeSystemAccount employeeSystemAccount)
        {
            EmployeeSystemAccount part = employeeSystemAccount.ToEmployeeSystemAccount();

            _context.employees.Add(part);
            await _context.SaveChangesAsync();

            return await GetEmployeeSystemAccount(Comon.GetMD5(part.EmployeeId.ToString()));
            // return CreatedAtAction("GetEmployeeSystemAccount", new { id = part.EmployeeId });
        }

        // DELETE: api/EmployeeSystemAccounts/md5-string
        [HttpDelete("{id}")]//删除
        public async Task<IActionResult> DeleteEmployeeSystemAccount(string id)
        {
            Guid ida = _context.encrypts.Where(e => e.EncryptionId == id).First().Id;
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
        {
            return _context.encrypts.Any(e => e.EncryptionId == id);
        }
    }
}