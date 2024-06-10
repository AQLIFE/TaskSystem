using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class CustomerController(ManagementSystemContext context, IMapper mapper) : ControllerBase
    {
        private CategoryActions categoryActions = new(context);
        private CustomerActions customerActions = new(context);

        [HttpGet]
        public async Task<PageContext<CustomerForSelect>?> GetCustomers(int page = 1, int pageSize = 120)
            => mapper.Map<PageContext<CustomerForSelect>>(await customerActions.SearchAsync(page, pageSize));

        [HttpGet("{hashName}")]
        public async Task<CustomerForSelect?> GetCustomerByName(string hashName)
        => await customerActions.ExistsCustomerByNameAsync(hashName)?mapper.Map<CustomerForSelect>(await customerActions.GetInfoAsync(e => e.CustomerName == hashName,t=>t.Categories)):null;

        [HttpPost]
        public async Task<bool> PostCustomer(CustomerForView info, int serial = 101)
            => !await customerActions.ExistsCustomerByNameAsync(info.CustomerName) && await categoryActions.ExistsCategoryBySerialAsync(serial)
                ? await customerActions.AddInfoAsync(mapper.Map<Customer>(info, opt => { opt.Items["Serial"] = serial; opt.Items["ManagementSystemContext"] = context; }) ) : false;



        // 更新客户信息，接受name
        [HttpPut("{hashName}")]
        public async Task<bool> UpdateCustomer(string hashName, CustomerForView info, int serial = 101)
            => hashName == info.CustomerName && await customerActions.ExistsCustomerByNameAsync(hashName) && !await categoryActions.ExistAsync(e => e.CategoryName == hashName)
                ? false : await customerActions.UpdateInfoAsync(mapper.Map<Customer>(info, opt => { opt.Items["Serial"] = serial; opt.Items["ManagementSystemContext"] = context; }));


        [HttpDelete("{hashName}")]
        public async Task<bool> DeleteCustomer(string hashName)
        => await customerActions.ExistsCustomerByNameAsync(hashName) ? await customerActions.DeleteInfoAsync(e => e.CustomerName == hashName):false;
    }
}