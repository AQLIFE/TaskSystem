using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class CategoryController(ManagementSystemContext storage, IMapper mapper) : ControllerBase
    {
        private CategoryRepositoryAsync CRA = new(storage);
        public enum CategoryType { all, level, parId }
        private const int DEFAULT_ADD = 0;





        [HttpGet, Authorize(policy: "Admin")]
        public async Task<PageContent<CategoryForSelect>?> GetCategorys(CategoryType select = 0, int obj = 1, int page = 1, int pageSize = SystemInfo.PageSize)
        {
            var x = select switch
            {
                CategoryType.all => await CRA.GetCategoryListAsync(page, pageSize),
                CategoryType.level => await CRA.GetCategoryListByLevelAsync(obj, page, pageSize),
                CategoryType.parId => await CRA.GetCategoryListByParIdAsync(obj, page, pageSize),
                _ => null
            };

            return x is not null ?
                 mapper.Map<PageContent<Category>, PageContent<CategoryForSelect>>(x)
            : null;
        }



        [HttpGet("{SortSerial}")]
        public async Task<CategoryForSelect?> GetCategory(int sortSerial)
            => mapper.Map<CategoryForSelect>(await CRA.GetCategoryBySerialAsync(sortSerial));


        [HttpPost]
        public async Task<bool> PostCategory(CategoryForAddOrUpdate add)
            => await CRA.AddCheckAsync(add) ? false
            : await CRA.AddAsync(mapper.Map<Category>(add, opt => { opt.Items["Serial"] = DEFAULT_ADD; opt.Items["ManagementSystemContext"] = storage; }));




        [HttpPut("{SortSerial}")]// PUT: api/categories/5  
        public async Task<bool> PutCategory(int sortSerial, CategoryForAddOrUpdate update)
            => await CRA.UpdateCheckAsync(sortSerial, update)
            ? await CRA.UpdateAsync(mapper.Map<Category>(update, opt => { opt.Items["Serial"] = sortSerial; opt.Items["ManagementSystemContext"] = storage; })) : false;




        [HttpDelete("{SortSerial}")]// DELETE: api/categories/5  
        public async Task<bool?> DeleteCategory(int SortSerial)
        {
            if (SortSerial != 100 && await CRA.TryGetCategoryBySerialAsync(SortSerial) is Category b)
            {
                b.CategoryLevel = 0;
                b.ParentCategory = await CRA.GetCategoryBySerialAsync(100);
                b.ParentCategoryId = b.ParentCategory?.CategoryId;

                return await CRA.UpdateAsync(b);

            }
            return false;
        }
    }
}