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
    public class CategoryController(ManagementSystemContext context) : ControllerBase
    {
        private CategoryActions action = new(context);
        public enum CategoryType { all, level, parId }

        [HttpGet, Authorize(policy: "Admin")]
        public IEnumerable<ICategoryInfo?>? GetCategorys(CategoryType select = 0, int obj = 1, int page = 1, int pageSize = 120)
        => select switch
        {
            CategoryType.all => action.GetCategoryList(page, pageSize).Select(e => e.ToCateInfo(action.GetSerialById(e.ParentCategoryId))),
            CategoryType.level => action.GetCategoryListByLevel(obj, page, pageSize).Select(e => e.ToCateInfo(action.GetSerialById(e.ParentCategoryId))),
            CategoryType.parId => action.GetCategoryListByParId(obj, page, pageSize).Select(e => e.ToCateInfo(action.GetSerialById(e.ParentCategoryId))),
            _ => null
        };


        [HttpPost]
        public async Task<ActionResult<string>> PostCategory(MiniCate info)
        {
            // if (!action.Validate(info)) return action.ValidateMessage;

            Category obj = info.ToCategory(action.GetIdBySerial(info.ParentSortSerial), action.GetLastSerial() + 1, action.GetLevelBySerial(info.ParentSortSerial));

            context.categories.Add(obj);
            await context.SaveChangesAsync();

            return obj.CategoryName;
        }


        [HttpGet("{SortSerial}")]
        public ActionResult<ICategoryInfo?> GetCategory(int SortSerial)
            => action.GetCategoryBySerial(SortSerial) is Category obj ? obj.ToCateInfo(action.GetParSerialBySerial(obj.SortSerial)) : null;

        // POST: api/categories  


        // PUT: api/categories/5  
        [HttpPut("{SortSerial}")]
        public async Task<string> PutCategory(int SortSerial, MiniCate info)
        {
            // if ( !action.ExistsCategoryBySerial(SortSerial)) return "不存在信息";

            Category? obj = action.GetCategoryBySerial(SortSerial);

            obj!.CategoryName = obj.CategoryName != info.CategoryName ? info.CategoryName : obj.CategoryName;
            obj.CategoryLevel = action.GetLevelBySerial(info.ParentSortSerial);
            obj.Remark = obj.Remark != info.Remark ? info.Remark : obj.Remark;
            var sl = action.GetCategoryBySerial(info.ParentSortSerial)!.CategoryId;
            obj.ParentCategoryId = obj.ParentCategoryId != sl ? sl : obj.ParentCategoryId;

            context.Entry(obj).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return "修改成功";
        }

        // DELETE: api/categories/5  
        [HttpDelete("{SortSerial}")]
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