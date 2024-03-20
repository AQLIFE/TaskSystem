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
    [ApiController, Route("api/[controller]"), Authorize(Roles = "99")]
    public class CategoryController(ManagementSystemContext context) : ControllerBase
    {
        private CategoryActions action = new(context);
        public enum CategoryType { all, level, parId }

        [HttpGet]
        public IEnumerable<ICategoryInfo?>? GetCategorys(CategoryType select = 0, int obj = 1, int page = 1, int pageSize = 120)
        => select switch
        {
            CategoryType.all =>   action.GetCategoryList(page, pageSize).Select(e=>e.ToCateInfo(action.GetSerialById(e.ParentCategoryId))),
            CategoryType.level => action.GetCategoryListByLevel(obj, page, pageSize).Select(e=>e.ToCateInfo(action.GetSerialById(e.ParentCategoryId))),
            CategoryType.parId => action.GetCategoryListByParId(obj, page, pageSize).Select(e=>e.ToCateInfo(action.GetSerialById(e.ParentCategoryId))),
            _ => null
        };


        [Obsolete]
        private IEnumerable<ICategoryInfo> MapToCategoryInfo(List<Category> obj)
        {
            List<ICategoryInfo> info = new();
            obj.ForEach(os => info.Add(CopyToCategory(os)));
            return info;
        }

        private ICategoryInfo CopyToCategory(Category obj)
            => obj.ToCateInfo(action.GetSerialById(obj.ParentCategoryId));


        [HttpPost]
        public async Task<ActionResult<string>> PostCategory(MiniCate info)
        {
            if (!action.Validate(info)) return action.ValidateMessage;

            Category obj = info.ToCategory(action.GetIdBySerial(info.ParentSortSerial), action.GetLastSerial() + 1,action.GetLevelBySerial(info.ParentSortSerial));

            context.categories.Add(obj);
            await context.SaveChangesAsync();

            return obj.CategoryName;
        }

        // [HttpGet("{id}/info")]
        // public ActionResult<int> GetLevel(int id) =>action.GetLevelById(id);

        [HttpGet("{id}")]
        public ActionResult<ICategoryInfo>? GetCategory(int id)
        {
            // if(!action.ExistsCategoryBySerial(id))return ;
            var obj = action.GetCategoryBySerial(id);
            if (obj is not null)
                return new CateInfo(obj, action.GetParSerialBySerial(obj.SortSerial));
            else return null;

        }

        // POST: api/categories  


        // PUT: api/categories/5  
        [HttpPut("{id}")]
        public async Task<string> PutCategory(int id, CateInfo category)
        {
            if ( !action.ExistsCategoryBySerial(id)) return "不存在信息";

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