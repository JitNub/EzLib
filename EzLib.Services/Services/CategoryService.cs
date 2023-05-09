using EzLib.Data;
using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Services
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

    }
}
