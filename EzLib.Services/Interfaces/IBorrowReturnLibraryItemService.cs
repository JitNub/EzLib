using EzLib.Models;

namespace EzLib.Services.Services
{
    public interface IBorrowReturnLibraryItemService
    {
        Task<LibraryItem> GetBorrowableLibraryItemAsync(int id);

        Task<bool> BorrowConfirmed(int id, string borrower);

        Task<LibraryItem> GetReturnableLibraryItemAsync(int id);

        Task<bool> ConfirmReturnAsync(int id);
    }
}