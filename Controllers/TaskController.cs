using AutoMapper;
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

    [ApiController, Authorize, Route("api/[controller]")]
    public class TaskSystemController : ControllerBase
    {

    }

    [ApiController, Authorize(Policy = "admin"), Route("api/[controller]")]
    public class CustomerController(ManagementSystemContext context,IMapper mapper) : ControllerBase
    {
        private CategoryActions categoryActions = new(context,mapper);
        private CustomerActions customerActions = new(context);

        [HttpGet]
        public ActionResult<IEnumerable<ICustomerInfo>> GetCustomers(int page = 1, int pageSize = 120)
            => context.customers.OrderByDescending(e => e.AddTime)
            .Skip((page - 1) * pageSize).Take(pageSize).ToList().Select(e => e as ICustomerInfo).ToList();


        [HttpPost]
        public async Task<string?> PostCustomer(MiniCustomer info, int serial = 101)
        {
            if (customerActions.ExistsCustomerByName(info.CustomerName)) return "客户已存在";
            else if (!await categoryActions.ExistsCategoryBySerialAsync(serial)) return "序列号不存在";

            Category? obj = categoryActions.GetCategoryBySerial(serial);
            Customer? ls = info.ToCustomer(obj!.CategoryId) as Customer;

            if (ls is Customer)
            {
                context.customers.Add(ls);
                await context.SaveChangesAsync();
                return info.CustomerName;
            }
            return "添加失败";
        }

        [HttpGet("{hashName}")]
        public async Task<ActionResult<Customer>> GetCustomerByHashedId(string hashName)
        {
            var customer = await context.customers.ToListAsync();

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // 更新客户信息，接受SHA512哈希过的GUID
        [HttpPut("{hashName}")]
        public async Task<IActionResult> UpdateCustomer(string hashName, Customer info)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCustomer = await context.customers.FindAsync(hashName);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            context.Entry(existingCustomer).CurrentValues.SetValues(info);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{hashName}")]
        public async Task<IActionResult> DeleteCustomer(string hashName)
        {
            var customer = await context.customers.FindAsync(hashName);

            if (customer == null)
            {
                return NotFound();
            }

            context.customers.Remove(customer);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}