using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzLib.Models
{
    public class LibraryItem
    {
        // Id property of the LibraryItem
        public int Id { get; set; }

        // CategoryId property with Required data annotation
        [Required(ErrorMessage = "The Category field is required.")]
        public int CategoryId { get; set; }

        // Title property with data annotations
        [StringLength(255, ErrorMessage = "Title must be between 1 and 255 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[^<>&'\""\/|{}[\]*%\\\\^]+$", ErrorMessage = "Title should not contain any special characters or symbols.")]
        public string Title { get; set; }

        // Author property with data annotations
        [StringLength(255, ErrorMessage = "Author must be between 1 and 255 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[^<>&'\""\/|{}[\]*%\\\\^]+$", ErrorMessage = "Author should not contain any special characters or symbols.")]
        public string Author { get; set; } = string.Empty;

        // Pages property with Range data annotation
        [Range(1, 10000, ErrorMessage = "Pages must be between 1 and 10000.")]
        public int? Pages { get; set; }

        // RunTimeMinutes property with Range data annotation
        [Range(1, 10000, ErrorMessage = "RunTimeMinutes must be between 1 and 10000.")]
        public int? RunTimeMinutes { get; set; }

        // IsBorrowable property
        public bool IsBorrowable { get; set; }

        // Borrower property with data annotations
        [StringLength(255, ErrorMessage = "Borrower must be between 1 and 255 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[^<>&'\""\/|{}[\]*%\\\\^]+$", ErrorMessage = "Borrower should not contain any special characters or symbols.")]
        public string Borrower { get; set; } = string.Empty;

        // BorrowDate property
        public DateTime? BorrowDate { get; set; }

        // Type property
        public string Type { get; set; }

        // Navigation property to Category, annotated with ForeignKey
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
