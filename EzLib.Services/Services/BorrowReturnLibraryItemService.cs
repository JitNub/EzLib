using EzLib.Data;
using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Services.Services
{
    public class BorrowReturnLibraryItemService : IBorrowReturnLibraryItemService
    {
        private readonly EzLibContext _context;
        private readonly IAcronymGeneratorService _acronymGeneratorService;

        // Constructor that injects EzLibContext and IAcronymGeneratorService dependencies
        public BorrowReturnLibraryItemService(EzLibContext context, IAcronymGeneratorService acronymGeneratorService)
        {
            _context = context;
            _acronymGeneratorService = acronymGeneratorService;
        }

        // Retrieves a borrowable library item by ID
        public async Task<LibraryItem> GetBorrowableLibraryItemAsync(int id)
        {
            return await _context.LibraryItem
                .Include(l => l.Category)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsBorrowable && string.IsNullOrEmpty(m.Borrower));
        }

        // Confirms the borrowing of a library item
        public async Task<bool> BorrowConfirmed(int id, string borrower)
        {
            var libraryItem = await _context.LibraryItem.FindAsync(id);

            if (libraryItem == null)
            {
                return false;
            }

            if (!libraryItem.IsBorrowable || !string.IsNullOrEmpty(libraryItem.Borrower))
            {
                return false;
            }

            if (string.IsNullOrEmpty(borrower))
            {
                return false;
            }

            libraryItem.Borrower = borrower;
            libraryItem.BorrowDate = DateTime.Now;
            libraryItem.IsBorrowable = false;

            await _context.SaveChangesAsync();

            return true;
        }

        // Retrieves a returnable library item by ID
        public async Task<LibraryItem> GetReturnableLibraryItemAsync(int id)
        {
            return await _context.LibraryItem.FindAsync(id);
        }

        // Confirms the return of a library item
        public async Task<bool> ConfirmReturnAsync(int id)
        {
            var libraryItem = await _context.LibraryItem.FindAsync(id);

            if (libraryItem == null || string.IsNullOrEmpty(libraryItem.Borrower))
            {
                return false;
            }

            libraryItem.Borrower = string.Empty;
            libraryItem.BorrowDate = null;
            libraryItem.IsBorrowable = true;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
