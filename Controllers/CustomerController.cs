using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Models;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Repository;
using TaskManangerSystem.Services.Tool;

namespace TaskManangerSystem.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class CustomerController(ManagementSystemContext context, IMapper mapper) : ControllerBase
    {
        private CategoryRepositoryAsync CRA = new(context);
        private CustomerRepositoryAsync CuRA = new(context);



        [HttpGet]
        public async Task<PageContent<CustomerForSelect>?> GetCustomers(int page = 1, int pageSize = SystemInfo.PageSize)
            => mapper.Map<PageContent<CustomerForSelect>>(await CuRA.SearchAsync(page, pageSize));

        [HttpGet("{hashName}")]
        public async Task<CustomerForSelect?> GetCustomerByName(string hashName)
        => await CuRA.ExistsCustomerByNameAsync(hashName) ? mapper.Map<CustomerForSelect>(await CuRA.TryGetAsync(e => e.CustomerName == hashName)) : null;

        [HttpPost]
        public async Task<bool> PostCustomer(CustomerForAddOrUpdate info, int serial = SystemInfo.CUSTOMER)
            => !await CuRA.ExistsCustomerByNameAsync(info.CustomerName) && await CRA.ExistsCategoryBySerialAsync(serial)
                ? await CuRA.AddAsync(mapper.Map<Customer>(info, opt => { opt.Items["Serial"] = serial; opt.Items["ManagementSystemContext"] = context; })) : false;



        // 更新客户信息，接受name
        [HttpPut("{hashName}")]
        public async Task<bool> UpdateCustomer(string hashName, CustomerForAddOrUpdate info, int serial = SystemInfo.CUSTOMER)
            => hashName == info.CustomerName && await CuRA.ExistsCustomerByNameAsync(hashName) && !await CRA.ExistAsync(e => e.CategoryName == hashName)
                ? false : await CuRA.UpdateAsync(mapper.Map<Customer>(info, opt => { opt.Items["Serial"] = serial; opt.Items["ManagementSystemContext"] = context; }));


        [HttpDelete("{hashName}")]
        public async Task<bool> DeleteCustomer(string hashName)
        => await CuRA.ExistsCustomerByNameAsync(hashName) ? await CuRA.DeleteAsync(e => e.CustomerName == hashName) : false;
    }
}