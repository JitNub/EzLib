using EzLib.Data;
using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Data
{
    public class DbSeeder : IDbSeeder
    {
        private readonly EzLibContext _context;

        public DbSeeder(EzLibContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            if (!await _context.Category.AnyAsync())
            {
                var categories = CreateCategories();
                await _context.Category.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }

            if (!await _context.LibraryItem.AnyAsync())
            {
                var categories = await _context.Category.ToListAsync();
                var items = CreateLibraryItems(categories);
                await _context.LibraryItem.AddRangeAsync(items);
                await _context.SaveChangesAsync();
            }
        }

        private static List<Category> CreateCategories()
        {
            return new List<Category>
            {
                new Category { CategoryName = "Fiction" },
                new Category { CategoryName = "Non-Fiction" },
                new Category { CategoryName = "Science Fiction" },
                new Category { CategoryName = "Biography" }
            };
        }

        private static List<LibraryItem> CreateLibraryItems(List<Category> categories)
        {
            return new List<LibraryItem>
            {
                // Books
                    new LibraryItem
                    {
                        Title = "The Great Gatsby",
                        Author = "F. Scott Fitzgerald",
                        Pages = 180,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "Book",
                        Category = categories[0]
                    },
                    new LibraryItem
                    {
                        Title = "To Kill a Mockingbird",
                        Author = "Harper Lee",
                        Pages = 281,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "Book",
                        Category = categories[0]
                    },
                    // DVDs
                    new LibraryItem
                    {
                        Title = "The Matrix",
                        Author = "Lana Wachowski",
                        RunTimeMinutes = 136,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "DVD",
                        Category = categories[2]
                    },
                    new LibraryItem
                    {
                        Title = "Inception",
                        Author = "Christopher Nolan",
                        RunTimeMinutes = 148,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "DVD",
                        Category = categories[2]
                    },
                    // Reference Books
                    new LibraryItem
                    {
                        Title = "The Oxford English Dictionary",
                        Author = "Oxford University Press",
                        Pages = 2176,
                        IsBorrowable = false,
                        Type = "Reference Book",
                        Category = categories[1]
                    },
                    new LibraryItem
                    {
                        Title = "Grays Anatomy",    // removed '
                        Author = "Henry Gray",
                        Pages = 1600,
                        IsBorrowable = false,
                        Type = "Reference Book",
                        Category = categories[3]
                    },
                    // Audiobooks
                    new LibraryItem
                    {
                        Title = "The Hitchhikers Guide to the Galaxy",      // removed '
                        Author = "Douglas Adams",
                        RunTimeMinutes = 324,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "Audiobook",
                        Category = categories[2]
                    },
                    new LibraryItem
                    {
                        Title = "Sapiens: A Brief History of Humankind",
                        Author = "Yuval Noah Harari",
                        RunTimeMinutes = 900,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "Audiobook",
                        Category = categories[1]
                    }
            };
        }
    }
}
