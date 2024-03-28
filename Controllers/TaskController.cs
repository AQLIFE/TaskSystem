using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [ApiController,Authorize , Route("api/[controller]")]
    public class CustomerController(ManagementSystemContext context) : ControllerBase
    {
        private CategoryActions categoryActions = new(context);
        private CustomerActions customerActions = new(context);

        [HttpGet,Authorize(Policy="admin")]
        public ActionResult<IEnumerable<ICustomerInfo>> GetCustomers(int page = 1, int pageSize = 120)
            => context.customers.OrderByDescending(e => e.AddTime)
            .Skip((page - 1) * pageSize).Take(pageSize).ToList().Select(e => e as ICustomerInfo).ToList();


        [HttpPost]
        public async Task<string?> PostCustomer(CustomerInfo info,int serial=101)
        {
            if (customerActions.ExistsCustomerByName(info.CustomerName)) return "客户已存在";
            else if(!categoryActions.ExistsCategoryBySerial(serial))return "序列号不存在";
            
            Category? obj = categoryActions.GetCategoryBySerial(serial);
            Customer? ls = info.ToCustomer(obj!.CategoryId) as Customer;

            if (ls is not null)
            {
                context.customers.Add(ls);
                await context.SaveChangesAsync();
                return info.CustomerName;
            }
            return "添加失败";
        }

        [HttpGet("{name}")]
        public ICustomerInfo? GetCustomer(string name)
        {

            if(!customerActions.ExistsCustomerByName(name))return null;
            else return customerActions.GetCustomerByName(name);
        }

        // [HttpPut("{name}")]
        // public string? PutCustomer(string name,BaseCustomerInfo customerInfo){
        //     if (customerActions.ExistsCustomerByName(name) && customerActions.GetCustomerByName(name) is TaskCustomer ov && ov != null)
                
        // }
    }
}