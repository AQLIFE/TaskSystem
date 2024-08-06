using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Repository;
using TaskManangerSystem.Tool;

namespace TaskManangerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(ManagementSystemContext storage, IMapper mapper) : ControllerBase
    {
        private InventoryRepositoryAsync IRA = new(storage);
        [HttpGet]
        public async Task<PageContent<InventoryForAddOrUpdate>?> GetInventoryList(int pageIndex = 1, int pageSize = SystemInfo.PageSize)
        => mapper.Map<PageContent<InventoryForAddOrUpdate>?>(await IRA.SearchAsync(pageIndex, pageSize));

        [HttpGet("info")]
        public async Task<InventoryForAddOrUpdate?> GetInventory(string name)
        => await IRA.ExistsInventoryByNameAsync(name) ? mapper.Map<InventoryForAddOrUpdate>(await IRA.GetInventoryByNameAsync(name)) : null;


        [HttpPost("info")]
        public async Task<bool> PostInventory(InventoryForAddOrUpdate view)
        {
            if (await IRA.ExistsInventoryByNameAsync(view.ProductName))
                return false;
            else
            {
                InventoryInfo x = mapper.Map<InventoryInfo>(view, opt =>
                {
                    opt.Items["ManagementSystemContext"] = storage; opt.Items["name"] = view.ProductName;
                });
                return await IRA.AddAsync(x);
            }
        }

        [HttpPut("info")]
        public async Task<bool> PutInventory(string name, InventoryForAddOrUpdate view)
            => await IRA.ExistsInventoryByNameAsync(view.ProductName) ? false : await IRA.UpdateAsync(mapper.Map<InventoryInfo>(view, opt => { opt.Items["ManagementSystemContext"] = storage; opt.Items["name"] = name; }));


        [HttpDelete("info")]
        public async Task<bool> DeleteInventory(string name)
        => await IRA.DeleteAsync(e => e.ProductName == name);
    }
}
