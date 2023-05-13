using EzLib.Models;

namespace EzLib.Services.Services
{
    public interface ILibraryItemValidationService
    {
        Dictionary<string, string> ValidateLibraryItem(LibraryItem libraryItem);
    }
}