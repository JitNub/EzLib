using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Data
{
    public class DbSeeder : IDbSeeder
    {
        private readonly EzLibContext _context;

        // Constructor takes an instance of the database context
        public DbSeeder(EzLibContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Apply any pending migrations
            await _context.Database.MigrateAsync();

            // Seed the database with categories if none are present
            if (!await _context.Category.AnyAsync())
            {
                var categories = CreateCategories();
                await _context.Category.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }

            // Seed the database with library items if none are present
            if (!await _context.LibraryItem.AnyAsync())
            {
                // Get the list of categories from the database
                var categories = await _context.Category.ToListAsync();

                // Create the library items
                var items = CreateLibraryItems(categories);

                // Add the library items to the database
                await _context.LibraryItem.AddRangeAsync(items);
                await _context.SaveChangesAsync();
            }
        }

        // Create a list of sample categories
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

        // Create a list of sample library items
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
                        Title = "Grays Anatomy",
                        Author = "Henry Gray",
                        Pages = 1600,
                        IsBorrowable = false,
                        Type = "Reference Book",
                        Category = categories[3]
                    },
                    // Audiobooks
                    new LibraryItem
                    {
                        Title = "The Hitchhikers Guide to the Galaxy",
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
