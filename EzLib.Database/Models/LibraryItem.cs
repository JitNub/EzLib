using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzLib.Models
{
    public class LibraryItem
    {
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [StringLength(255, ErrorMessage = "Title must be between 1 and 255 characters.", MinimumLength = 1)]
        //[RegularExpression(@"^[\p{L}\s.'-äåöÄÅÖ\d]+[\p{L}\s.'-äåöÄÅÖ\d]*$", ErrorMessage = "The title name must match the format.")]
        public string Title { get; set; }

        [StringLength(255, ErrorMessage = "Author must be between 1 and 255 characters.", MinimumLength = 1)]
        //[RegularExpression(@"^[\p{L}\s.'-äåöÄÅÖ]+[\s][\p{L}\s.'-äåöÄÅÖ]+$", ErrorMessage = "The author name must match the full name format.")]
        public string Author { get; set; } = String.Empty;

        [Range(1, 10000, ErrorMessage = "Pages must be between 1 and 10000.")]
        public int? Pages { get; set; }

        [Range(1, 10000, ErrorMessage = "RunTimeMinutes must be between 1 and 10000.")]
        public int? RunTimeMinutes { get; set; }

        public bool IsBorrowable { get; set; }

        [StringLength(255, ErrorMessage = "Borrower must be between 1 and 255 characters.", MinimumLength = 1)]
        //[RegularExpression(@"^[\p{L}\s.'-äåöÄÅÖ]+[\s][\p{L}\s.'-äåöÄÅÖ]+$", ErrorMessage = "The borrower name must match the full name format.")]
        public string Borrower { get; set; } = String.Empty;

        public DateTime? BorrowDate { get; set; }

        public string Type { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
