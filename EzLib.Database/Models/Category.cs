using System.ComponentModel.DataAnnotations;

namespace EzLib.Models
{
    public class Category
    {
        public Category()
        {
            LibraryItems = new HashSet<LibraryItem>();
        }

        public int Id { get; set; }

        [StringLength(255, ErrorMessage = "CategoryName must be between 1 and 255 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[^<>&'\""\/|{}[\]*%\\\\^]+$", ErrorMessage = "CategoryName should not contain any special characters or symbols.")]
        public string CategoryName { get; set; }

        public virtual ICollection<LibraryItem> LibraryItems { get; set; }
    }
}
