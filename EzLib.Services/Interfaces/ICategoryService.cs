using EzLib.Models;

namespace EzLib.Services.Services
{
    public interface ICategoryService
    {
        Task<bool> IsCategoryNameUnique(Category category);

        Task<bool> CanDeleteCategoryAsync(int categoryId);

        Task<List<Category>> GetCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(int? id);

        Task CreateCategoryAsync(Category category);

        Task<Category> GetCategoryAsync(int? id);

        Task<bool> UpdateCategoryAsync(Category category);

        bool CategoryExists(int id);
    }
}