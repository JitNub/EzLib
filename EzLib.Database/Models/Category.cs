using System.ComponentModel.DataAnnotations;

namespace EzLib.Models
{
    public class Category
    {
        // Constructor that initializes the LibraryItems collection
        public Category()
        {
            LibraryItems = new HashSet<LibraryItem>();
        }

        // Id property of the Category
        public int Id { get; set; }

        // CategoryName property with data annotations
        [StringLength(255, ErrorMessage = "CategoryName must be between 1 and 255 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[^<>&'\""\/|{}[\]*%\\\\^]+$", ErrorMessage = "CategoryName should not contain any special characters or symbols.")]
        public string CategoryName { get; set; }

        // Collection navigation property to LibraryItems
        public virtual ICollection<LibraryItem> LibraryItems { get; set; }
    }
}
