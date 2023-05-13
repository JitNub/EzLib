using EzLib.Models;

namespace EzLib.Services.Services
{
    public interface IBlockedFieldClearingService
    {
        void ClearBlockedFields(LibraryItem libraryItem);
    }
}