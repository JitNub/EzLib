using EzLib.Models;

namespace EzLib.Services.Services
{
    public class BlockedFieldClearingService : IBlockedFieldClearingService
    {
        public void ClearBlockedFields(LibraryItem libraryItem)
        {
            if (libraryItem.Type == "Book")
            {
                libraryItem.RunTimeMinutes = null;
            }
            else if (libraryItem.Type == "DVD")
            {
                libraryItem.Author = String.Empty;
                libraryItem.Pages = null;
            }
            else if (libraryItem.Type == "Audio Book")
            {
                libraryItem.Author = String.Empty;
                libraryItem.Pages = null;
            }
            else if (libraryItem.Type == "Reference Book")
            {
                libraryItem.RunTimeMinutes = null;
            }
        }
    }
}
