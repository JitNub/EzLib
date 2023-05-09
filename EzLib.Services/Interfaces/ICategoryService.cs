using EzLib.Models;

namespace EzLib.Services
{
    public interface ICategoryService
    {
        Task<bool> IsCategoryNameUnique(Category category);

        Task<bool> CanDeleteCategoryAsync(int categoryId);
    }
}