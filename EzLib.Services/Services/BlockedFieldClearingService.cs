using EzLib.Models;

namespace EzLib.Services.Services
{
    public class BlockedFieldClearingService : IBlockedFieldClearingService
    {
        public void ClearBlockedFields(LibraryItem libraryItem)
        {
            if (libraryItem.Type == "Book")
            {
                libraryItem.Author = null;
                libraryItem.Pages = null;
                libraryItem.RunTimeMinutes = null;
            }
            else if (libraryItem.Type == "DVD")
            {
                libraryItem.Author = null;
                libraryItem.Pages = null;
            }
            else if (libraryItem.Type == "Audio Book")
            {
                libraryItem.Author = null;
                libraryItem.Pages = null;
            }
            else if (libraryItem.Type == "Reference Book")
            {
                libraryItem.RunTimeMinutes = null;
            }
        }
    }
}
