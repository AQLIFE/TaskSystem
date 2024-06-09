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
    public class CategoryController(ManagementSystemContext context, IMapper mapper) : ControllerBase
    {
        private CategoryActions action = new(context);
        public enum CategoryType { all, level, parId }





        [HttpGet, Authorize(policy: "Admin")]
        public async Task<PageContext<CategoryForSelectOrUpdate>?> GetCategorys(CategoryType select = 0, int obj = 1, int page = 1, int pageSize = 120)
        {
            var x = select switch
            {
                CategoryType.all => await action.GetCategoryListAsync(page, pageSize),
                CategoryType.level => await action.GetCategoryListByLevelAsync(obj, page, pageSize),
                CategoryType.parId => await action.GetCategoryListByParIdAsync(obj, page, pageSize),
                _ => null
            };

            if (x is not null)
            {
                List<CategoryForSelectOrUpdate> t = mapper.Map<List<CategoryForSelectOrUpdate>>(x.data);

                return new(x.pageIndex, x.MaxPage, x.Sum, t);
            }
            else return null;
        }



        [HttpGet("{SortSerial}")]
        public async Task<CategoryForSelectOrUpdate?> GetCategory(int sortSerial)
            => mapper.Map<CategoryForSelectOrUpdate>(await action.GetCategoryBySerialAsync(sortSerial));


        [HttpPost]
        public async Task<bool> PostCategory(CategoryForAdd add)
            =>await action.ExistsCategoryByNameAsync(add.CategoryName) ? false : action.AddInfo(mapper.Map<Category>(add, opt => opt.Items["ManagementSystemContext"]=context) );
        



        [HttpPut("{SortSerial}")]// PUT: api/categories/5  
        public async Task<string> PutCategory(int sortSerial, CategoryForSelectOrUpdate info)
        {
            // if ( !action.ExistsCategoryBySerial(SortSerial)) return "不存在信息";

            Category? obj = action.GetCategoryBySerial(sortSerial);

            obj!.CategoryName = obj.CategoryName != info.CategoryName ? info.CategoryName : obj.CategoryName;
            obj.CategoryLevel = await action.GetLevelBySerialAsync(info.ParentSortSerial);
            obj.Remark = obj.Remark != info.Remark ? info.Remark : obj.Remark;
            var sl = action.GetCategoryBySerial(info.ParentSortSerial)!.CategoryId;
            obj.ParentCategoryId = obj.ParentCategoryId != sl ? sl : obj.ParentCategoryId;

            context.Entry(obj).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return "修改成功";
        }




        [HttpDelete("{SortSerial}")]// DELETE: api/categories/5  
        public async Task<string?> DeleteCategory(int SortSerial)
        {
            var category = action.GetCategoryBySerial(SortSerial);
            if (category == null) return "不存在信息";

            context.categories.Remove(category);
            await context.SaveChangesAsync();

            return "删除成功";
        }
    }
}