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

        [HttpGet]
        public async Task<IEnumerable<BaseCateInfo?>> GetCategory(int page = 1, int pageSize = 120)
        // => await context.categories.OrderBy(c => c.CategoryLevel).Skip((page - 1) * pageSize).Take(pageSize).Select(e=>e.ToCateInfo(action.GetCategory((Guid)e.ParentCategoryId!)!.SortSerial)).ToListAsync();
        => await context.categories.OrderBy(c => c.CategoryLevel).OrderBy(c => c.SortSerial)
        .Skip((page - 1) * pageSize).Take(pageSize)
        .Select(  e =>  e.ToCateInfo(  action.GetParentSort((Guid)e.ParentCategoryId!).Result)).ToListAsync();

        // [HttpGet("level")]
        // public async Task<IEnumerable<BaseCateInfo?>> GetCategory(int level = 1, int page = 1, int pageSize = 120)
        // {
        //     Console.WriteLine(level);
        //     return await context.categories
        //     .Where(e => e.CategoryLevel == level)
        //     .OrderBy(c => c.SortSerial)
        //     .Skip((page - 1) * pageSize).Take(pageSize)
        //     .Select(e => e.ToCateInfo(action.GetParentSort((Guid)e.ParentCategoryId!))).ToListAsync();

        // }

        [HttpPost]
        public async Task<ActionResult<string>> PostCategory(CateInfo category)
        {
            // if (action.ExitsCategoryBySerial(category.SortSerial)) return "已存在同序列分类";
            // else if( category.ParentSortSerial !=0 && !action.ExitsCategoryBySerial(category.ParentSortSerial))return "父分类序列号不存在";
            if (!action.Validate(category)) return action.ValidateMessage;

            Category obj = category.ToCategory(Guid.NewGuid(), action.GetParentId(category.ParentSortSerial));

            // Console.WriteLine($"{obj.CategoryId},{obj.CategoryLevel},{obj.CategoryName},{obj.ParentCategoryId},{obj.SortSerial}");

            context.categories.Add(obj);
            await context.SaveChangesAsync();

            return obj.CategoryName;
        }

        [HttpGet("{id}")]
        public ActionResult<BaseCateInfo?> GetCategoryById(int id)
            => action.GetCategoryBySerial(id)?.ToCateInfo(id);

        // POST: api/categories  


        // PUT: api/categories/5  
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            context.Entry(category).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/categories/5  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            context.categories.Remove(category);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}