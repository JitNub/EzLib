using EzLib.Data;
using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Services.Services
{
    public class LibraryItemsService : ILibraryItemsService
    {
        private readonly EzLibContext _context;
        private readonly IAcronymGeneratorService _acronymGeneratorService;

        // Constructor that injects EzLibContext and IAcronymGeneratorService dependencies
        public LibraryItemsService(EzLibContext context, IAcronymGeneratorService acronymGeneratorService)
        {
            _context = context;
            _acronymGeneratorService = acronymGeneratorService;
        }

        // Checks if the library item title is unique
        public async Task<bool> IsLibraryItemTitleUnique(LibraryItem libraryItem)
        {
            return await _context.LibraryItem.AllAsync(li => li.Id == libraryItem.Id || li.Title != libraryItem.Title);
        }

        // Retrieves library items based on sorting type and search string
        public async Task<(IQueryable<LibraryItem> libraryItems, string sortByType)> GetLibraryItemsAsync(string sortByType, string searchString)
        {
            IQueryable<LibraryItem> ezLibContext = _context.LibraryItem.Include(l => l.Category).AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                ezLibContext = ezLibContext.Where(li => li.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            if (sortByType == "Type")
            {
                ezLibContext = ezLibContext.OrderBy(l => l.Type);
            }
            else
            {
                ezLibContext = ezLibContext.OrderBy(l => l.Category.CategoryName);
            }

            var libraryItems = await ezLibContext.ToListAsync();

            foreach (var libraryItem in libraryItems)
            {
                string acronym = _acronymGeneratorService.GenerateAcronym(libraryItem.Title);
                libraryItem.Title = $"{acronym}";
            }

            return (libraryItems.AsQueryable(), sortByType);
        }

        // Retrieves library item details by ID
        public async Task<LibraryItem> GetLibraryItemDetailsAsync(int? id)
        {
            if (id == null || _context.LibraryItem == null)
            {
                return null;
            }

            var libraryItem = await _context.LibraryItem
                .Include(l => l.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libraryItem != null)
            {
                string acronym = _acronymGeneratorService.GenerateAcronym(libraryItem.Title);
                libraryItem.Title = $"{acronym}";
            }

            return libraryItem;
        }

        // Creates a new library item
        public async Task<LibraryItem> CreateLibraryItemAsync(LibraryItem libraryItem)
        {
            _context.Add(libraryItem);
            await _context.SaveChangesAsync();
            return libraryItem;
        }

        // Retrieves a library item by ID
        public async Task<LibraryItem> GetLibraryItemAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var libraryItem = await _context.LibraryItem.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return libraryItem;
        }

        // Updates a library item
        public async Task<bool> UpdateLibraryItemAsync(int id, LibraryItem libraryItem)
        {
            if (id != libraryItem.Id)
            {
                return false;
            }

            try
            {
                _context.Update(libraryItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryItemExists(libraryItem.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        // Checks if a library item with the given ID exists
        private bool LibraryItemExists(int id)
        {
            return (_context.LibraryItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Retrieves library item details for deletion by ID
        public async Task<LibraryItem> GetLibraryItemForDeleteAsync(int? id)
        {
            if (id == null || _context.LibraryItem == null)
            {
                return null;
            }

            var libraryItem = await _context.LibraryItem
                .Include(l => l.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return libraryItem;
        }

        // Deletes a library item by ID
        public async Task<bool> DeleteLibraryItemAsync(int id)
        {
            if (_context.LibraryItem == null)
            {
                throw new InvalidOperationException("Entity set 'EzLibContext.LibraryItem' is null.");
            }

            var libraryItem = await _context.LibraryItem.FindAsync(id);
            if (libraryItem != null)
            {
                _context.LibraryItem.Remove(libraryItem);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
