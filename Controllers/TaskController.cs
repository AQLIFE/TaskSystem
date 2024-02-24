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
    public class TaskSystem : ControllerBase
    {

    }

    [ApiController, Authorize(Roles = "99"), Route("api/[controller]")]
    public class CustomerController(ManagementSystemContext context) : ControllerBase
    {
        private CategoryActions action = new(context);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ICustomerInfo>>> GetCustomers(int page = 1, int pageSize = 120)
            => await context.customers.OrderByDescending(e => e.AddTime)
            .Skip((page - 1) * pageSize).Take(pageSize).ToList().Select(e => e.ToCustomerInfo()).ToListAsync();


        [HttpPost]
        public async Task<string?> PostCustomer(BaseCustomerInfo customer)
        {
            var obj = action.ExistsCategoryBySerial(customer.serial) ? action.GetCategoryBySerial() : action.GetCategoryBySerial(102);
            var ls = customer.ToCustomer(obj!.CategoryId) as TaskCustomer;
            if (ls != null)
            {
                context.customers.Add(ls);
                await context.SaveChangesAsync();
                return customer.CustomerName;
            }
            return "失败";   
        }
    }
}