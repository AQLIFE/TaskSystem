using TaskManangerSystem.Services;
using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.SystemBean;
using AutoMapper;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(ManagementSystemContext storage, IMapper mapper) : ControllerBase
    {
        private InventoryActions inventoryActions = new(storage);
        [HttpGet]
        public async Task<PageContext<InventoryForView>?> GetInventoryList(int pageIndex = 1, int pageSize = 100)
        => mapper.Map<PageContext<InventoryForView>?>(await inventoryActions.SearchAsync(pageIndex, pageSize));

        [HttpGet("info")]
        public async Task<InventoryForView?> GetInventory(string name)
        => await inventoryActions.ExistsInventoryByNameAsync(name) ? mapper.Map<InventoryForView>(await inventoryActions.GetInventoryByNameAsync(name)) : null;


        [HttpPost("info")]
        public async Task<bool> PostInventory(InventoryForView view)
        => await inventoryActions.ExistsInventoryByNameAsync(view.ProductName) ? false : await inventoryActions.AddInfoAsync(mapper.Map<InventoryInfo>(await inventoryActions.GetInventoryByNameAsync(view.ProductName), opt => opt.Items["ManagementSystemContext"] = storage));


        [HttpPut("info")]
        public async Task<bool> PutInventory(string name, InventoryForView view)
        => await inventoryActions.ExistsInventoryByNameAsync(view.ProductName) ? false : await inventoryActions.UpdateInfoAsync(mapper.Map<InventoryInfo>(view, opt => { opt.Items["ManagementSystemContext"] = storage; opt.Items["name"] = name; }));


        [HttpDelete("info")]
        public async Task<bool> DeleteInventory(string name)
        => await inventoryActions.DeleteInfoAsync(e => e.ProductName == name);
    }
}
