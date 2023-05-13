using EzLib.Data;
using EzLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzLib.Services.Services
{
    public class BorrowReturnLibraryItemService : IBorrowReturnLibraryItemService
    {
        private readonly EzLibContext _context;
        private readonly IAcronymGeneratorService _acronymGeneratorService;

        public BorrowReturnLibraryItemService(EzLibContext context, IAcronymGeneratorService acronymGeneratorService)
        {
            _context = context;
            _acronymGeneratorService = acronymGeneratorService;
        }

        public async Task<LibraryItem> GetBorrowableLibraryItemAsync(int id)
        {
            return await _context.LibraryItem
                .Include(l => l.Category)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsBorrowable && string.IsNullOrEmpty(m.Borrower));
        }

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

        public async Task<LibraryItem> GetReturnableLibraryItemAsync(int id)
        {
            return await _context.LibraryItem.FindAsync(id);
        }

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
