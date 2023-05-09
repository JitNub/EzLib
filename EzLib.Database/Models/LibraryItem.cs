using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzLib.Models
{
    public class LibraryItem
    {
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int? Pages { get; set; }

        public int? RunTimeMinutes { get; set; }

        public bool IsBorrowable { get; set; }

        public string Borrower { get; set; }

        public DateTime? BorrowDate { get; set; }

        public string Type { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
