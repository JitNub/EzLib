using EzLib.Data;
using EzLib.Models;
using Microsoft.EntityFrameworkCore;
namespace EzLib.Services.Services
{
    public class LibraryItemsService : ILibraryItemsService
    {
        private readonly EzLibContext _context;
        private readonly IAcronymGeneratorService _acronymGeneratorService;

        public LibraryItemsService(EzLibContext context, IAcronymGeneratorService acronymGeneratorService)
        {
            _context = context;
            _acronymGeneratorService = acronymGeneratorService;
        }

        public async Task<(IQueryable<LibraryItem> libraryItems, string sortByType)> GetLibraryItemsAsync(string sortByType)
        {
            IQueryable<LibraryItem> ezLibContext = _context.LibraryItem.Include(l => l.Category);

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

        public async Task<LibraryItem> GetLibraryItemDetailsAsync(int? id)
        {
            if (id == null || _context.LibraryItem == null)
            {
                return null;
            }

            var libraryItem = await _context.LibraryItem
                .Include(l => l.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libraryItem != null)
            {
                string acronym = _acronymGeneratorService.GenerateAcronym(libraryItem.Title);
                libraryItem.Title = $"{acronym}";
            }

            return libraryItem;
        }

        public async Task<LibraryItem> CreateLibraryItemAsync(LibraryItem libraryItem)
        {
            _context.Add(libraryItem);
            await _context.SaveChangesAsync();
            return libraryItem;
        }

        public async Task<LibraryItem> GetLibraryItemAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var libraryItem = await _context.LibraryItem.FindAsync(id);
            return libraryItem;
        }

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

        private bool LibraryItemExists(int id)
        {
            return (_context.LibraryItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<LibraryItem> GetLibraryItemForDeleteAsync(int? id)
        {
            if (id == null || _context.LibraryItem == null)
            {
                return null;
            }

            var libraryItem = await _context.LibraryItem
                .Include(l => l.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            return libraryItem;
        }

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
