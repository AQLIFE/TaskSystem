using AutoMapper;
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
    [ApiController, Route("api/[controller]"), Authorize]
    public class CategoryController(ManagementSystemContext context,IMapper mapper) : ControllerBase
    {
        private CategoryActions action = new(context,mapper);
        public enum CategoryType { all, level, parId }





        [HttpGet, Authorize(policy: "Admin")]     
        public async Task<PageContext<CategoryForSelectOrUpdate>?> GetCategorys(CategoryType select = 0, int obj = 1, int page = 1, int pageSize = 120)
        => select switch{
            CategoryType.all => await action.GetCategoryListAsync(page, pageSize),
            CategoryType.level => await action.GetCategoryListByLevelAsync(obj, page, pageSize),
            CategoryType.parId => await action.GetCategoryListByParIdAsync(obj, page, pageSize),
            _ => null};

        [HttpGet("{SortSerial}")]
        public async Task<CategoryForSelectOrUpdate?> GetCategory(int sortSerial)
            => await action.GetCategoryBySerialAsync(sortSerial) is Category ib ? mapper.Map<CategoryForSelectOrUpdate>(ib):null;
            // => action.GetCategoryBySerial(SortSerial) is Category obj ? obj.ToCateInfo(action.GetParSerialBySerial(obj.SortSerial)) : null;

        // [HttpPost]// POST: api/categories  
        // public async Task<ActionResult<string>> PostCategory(MiniCate info)
        // {
        //     // if (!action.Validate(info)) return action.ValidateMessage;

        //     Category obj = info.ToCategory(action.GetIdBySerial(info.ParentSortSerial), action.GetLastSerial() + 1, action.GetLevelBySerial(info.ParentSortSerial));

        //     context.categories.Add(obj);
        //     await context.SaveChangesAsync();

        //     return obj.CategoryName;
        // }


        [HttpPost]
        public async Task<bool> PostCategory(CategoryForAdd add)
        => await action.ExistsCategoryByNameAsync(add.CategoryName) ?false:action.AddInfo( mapper.Map<Category>(add) );
        //     //检查
        //     if( await action.ExistsCategoryByNameAsync(add.CategoryName) )return false;
        //     else 
        //         return action.AddInfo( mapper.Map<Category>(add) );    
        // }



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