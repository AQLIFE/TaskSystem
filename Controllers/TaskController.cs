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
        private CategoryActions categoryActions = new(context);
        private CustomerActions customerActions = new(context);

        [HttpGet]
        public ActionResult<IEnumerable<ICustomerInfo>> GetCustomers(int page = 1, int pageSize = 120)
            => context.customers.OrderByDescending(e => e.AddTime)
            .Skip((page - 1) * pageSize).Take(pageSize).ToList().Select(e => e.ToCustomerInfo()).ToList();


        [HttpPost]
        public async Task<string?> PostCustomer(BaseCustomerInfo customer)
        {
            var obj = categoryActions.GetCategoryBySerial(customer.Serial) ?? categoryActions.GetCategoryBySerial(101);
            TaskCustomer ls = customer.ToCustomer(obj!.CategoryId);
            if (ls != null)
            {
                context.customers.Add(ls);
                await context.SaveChangesAsync();
                return customer.CustomerName;
            }
            return "失败";
        }

        [HttpGet("{name}")]
        public ICustomerInfo? GetCustomer(string name)
        {
            if (customerActions.ExistsCustomerByName(name) && customerActions.GetCustomerByName(name) is TaskCustomer ov && ov != null)
                // TaskCustomer? ov = customerActions.GetCustomerByName(name);
                // if (ov != null)
                return ov.ToCustomerInfo(categoryActions.GetCategory(ov.CustomerType)!.SortSerial);
            else return null;
        }

        [HttpPut("{name}")]
        public string? PutCustomer(string name,BaseCustomerInfo customerInfo){
            if (customerActions.ExistsCustomerByName(name) && customerActions.GetCustomerByName(name) is TaskCustomer ov && ov != null)
                
        }
    }
}