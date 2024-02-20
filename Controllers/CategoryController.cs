using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;
using ZstdSharp.Unsafe;

namespace TaskManangerSystem.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class CategoryController(ManagementSystemContext context) : ControllerBase
    {
        private CategoryActions action = new(context);
        public enum CategoryType { all, level, parId }

        [HttpGet]
        public IEnumerable<BaseCateInfo?>? GetCategory(CategoryType select = 0, int obj = 1, int page = 1, int pageSize = 120)
        => select switch { CategoryType.all => action.GetCategoryList(page, pageSize), CategoryType.level => action.GetCategoryListByLevel(obj, page, pageSize), CategoryType.parId => action.GetCategoryListByParId(obj, page, pageSize), _ => null };

        [HttpPost]
        public async Task<ActionResult<string>> PostCategory(CaInfo category)
        {
            if (!action.Validate(category)) return action.ValidateMessage;

            Guid? iss = action.GetParentId(category.ParentSortSerial);
            int id = action.GetLastId() + 1;
            int level = action.GetLevelByParId((Guid)iss!)+1;

            Category obj = category.ToCategory(Guid.NewGuid(), iss, id, level);

            context.categories.Add(obj);
            await context.SaveChangesAsync();

            return obj.CategoryName;
        }

        // [HttpGet("{id}/info")]
        // public ActionResult<int> GetLevel(int id) =>action.GetLevelById(id);

        [HttpGet("{id}")]
        public ActionResult<BaseCateInfo?> GetCategoryById(int id)
        {
            var obj = action.GetCategoryBySerial(id);
            return obj?.ToCateInfo(action.GetParIdBySerial(id));
        }

        // POST: api/categories  


        // PUT: api/categories/5  
        [HttpPut("{id}")]
        public async Task<string> PutCategory(int id, CaInfo category)
        {
            if (id <= 0 || !action.ExistsCategoryBySerial(id)) return "不存在信息";

            Category? obj = action.GetCategoryBySerial(id);

            obj!.CategoryName = obj.CategoryName != category.CategoryName ? category.CategoryName : obj.CategoryName;
            obj.CategoryLevel = obj.CategoryLevel != category.CategoryLevel ? category.CategoryLevel : obj.CategoryLevel;
            obj.Remark = obj.Remark != category.Remark ? category.Remark : obj.Remark;
            var sl = action.GetCategoryBySerial(category.ParentSortSerial)!.CategoryId;
            obj.ParentCategoryId = obj.ParentCategoryId != sl ? sl : obj.ParentCategoryId;
            // obj.SortSerial = obj.SortSerial != category.SortSerial ? category.SortSerial : obj.SortSerial;
            context.Entry(obj).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return "修改成功";
        }

        // DELETE: api/categories/5  
        [HttpDelete("{id}")]
        public async Task<string?> DeleteCategory(int id)
        {
            var category = action.GetCategoryBySerial(id);
            if (category == null) return "不存在信息";

            context.categories.Remove(category);
            await context.SaveChangesAsync();

            return "删除成功";


        }
    }
}