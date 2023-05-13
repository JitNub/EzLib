using System.Collections.Generic;
using EzLib.Models;

namespace EzLib.Services.Services
{
    public class LibraryItemValidationService : ILibraryItemValidationService
    {
        public Dictionary<string, string> ValidateLibraryItem(LibraryItem libraryItem)
        {
            var errors = new Dictionary<string, string>();

            if (libraryItem.Type == "Book")
            {
                errors.Add("Borrower", "The Borrower field is not required for this item.");
                errors.Add("RunTimeMinutes", "The RunTimeMinutes field is not required for this item.");
                libraryItem.IsBorrowable = true;
            }
            else if (libraryItem.Type == "DVD")
            {
                errors.Add("Borrower", "The Borrower field is not required for this item.");
                errors.Add("Author", "The Author field is not required for this item.");
                errors.Add("Pages", "The Pages field is not required for this item.");
                libraryItem.IsBorrowable = true;
            }
            else if (libraryItem.Type == "Audio Book")
            {
                errors.Add("Borrower", "The Borrower field is not required for this item.");
                errors.Add("Author", "The Author field is not required for this item.");
                errors.Add("Pages", "The Pages field is not required for this item.");
                libraryItem.IsBorrowable = true;
            }
            else if (libraryItem.Type == "Reference Book")
            {
                errors.Add("Borrower", "The Borrower field is not required for this item.");
                errors.Add("RunTimeMinutes", "The RunTimeMinutes field is not required for this item.");
                libraryItem.IsBorrowable = false;
            }

            return errors;
        }
    }
}
