using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class CategoryController(ManagementSystemContext storage, IMapper mapper) : ControllerBase
    {
        private CategoryActions action = new(storage);
        public enum CategoryType { all, level, parId }





        [HttpGet, Authorize(policy: "Admin")]
        public async Task<PageContext<CategoryForSelect>?> GetCategorys(CategoryType select = 0, int obj = 1, int page = 1, int pageSize = 120)
        {
            var x = select switch
            {
                CategoryType.all => await action.GetCategoryListAsync(page, pageSize),
                CategoryType.level => await action.GetCategoryListByLevelAsync(obj, page, pageSize),
                CategoryType.parId => await action.GetCategoryListByParIdAsync(obj, page, pageSize),
                _ => null
            };

            return x is not null ?
                 mapper.Map<PageContext<Category>, PageContext<CategoryForSelect>>(x)
            :null;
        }



        [HttpGet("{SortSerial}")]
        public async Task<CategoryForSelect?> GetCategory(int sortSerial)
            => mapper.Map<CategoryForSelect>(await action.GetCategoryBySerialAsync(sortSerial));


        [HttpPost]
        public async Task<bool> PostCategory(CategoryForAddOrUpdate add)
            => await action.AddCheckAsync(add) ? false
            : action.AddInfo(mapper.Map<Category>(add, opt => { opt.Items["Serial"] = 0; opt.Items["ManagementSystemContext"] = storage; }));




        [HttpPut("{SortSerial}")]// PUT: api/categories/5  
        public async Task<bool> PutCategory(int sortSerial, CategoryForAddOrUpdate update)
            => await action.UpdateCheckAsync(sortSerial, update)
            ? await action.UpdateInfoAsync(mapper.Map<Category>(update, opt => { opt.Items["Serial"] = sortSerial; opt.Items["ManagementSystemContext"] = storage; })) : false;




        [HttpDelete("{SortSerial}")]// DELETE: api/categories/5  
        public async Task<bool?> DeleteCategory(int SortSerial)
        {
            if (await action.TryGetCategoryBySerialAsync(SortSerial) is Category b)
            {
                storage.categories.Remove(b);
                return (await storage.SaveChangesAsync()) == 1;
            }
            return false;
        }
    }
}