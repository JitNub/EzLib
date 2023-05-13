using EzLib.Data;
using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EzLibContext _context;

        public CategoryService(EzLibContext context)
        {
            _context = context;
        }

        public async Task<bool> IsCategoryNameUnique(Category category)
        {
            return await _context.Category.AllAsync(c => c.Id == category.Id || c.CategoryName != category.CategoryName);
        }

        public async Task<bool> CanDeleteCategoryAsync(int categoryId)
        {
            return !await _context.LibraryItem.AnyAsync(li => li.CategoryId == categoryId);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            if (_context.Category == null)
            {
                throw new InvalidOperationException("Entity set 'EzLibContext.Category' is null.");
            }
            else
            {
                return await _context.Category.OrderBy(c => c.CategoryName).ToListAsync();
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return null;
            }

            return await _context.Category.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryAsync(int? id)
        {
            return await _context.Category.FindAsync(id);
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
