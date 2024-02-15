using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class CategoryController(ManagementSystemContext context) : ControllerBase
    {
        [HttpGet("/{page}")]
        public async Task<ActionResult<IEnumerable<Category?>>> GetCategory(int page = 1, int pageSize = 120)
        => await context.categories.OrderBy(c => c.CategoryLevel).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Category?>> GetCategoryById(Guid id)
            => await context.categories.FindAsync(id);

        // POST: api/categories  
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            context.categories.Add(category);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

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