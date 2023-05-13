using EzLib.Models;

namespace EzLib.Services.Services
{
    public interface ILibraryItemsService
    {
        Task<(IQueryable<LibraryItem> libraryItems, string sortByType)> GetLibraryItemsAsync(string sortByType);

        Task<LibraryItem> GetLibraryItemDetailsAsync(int? id);

        Task<LibraryItem> CreateLibraryItemAsync(LibraryItem libraryItem);

        Task<LibraryItem> GetLibraryItemAsync(int? id);

        Task<bool> UpdateLibraryItemAsync(int id, LibraryItem libraryItem);

        Task<LibraryItem> GetLibraryItemForDeleteAsync(int? id);

        Task<bool> DeleteLibraryItemAsync(int id);

    }
}