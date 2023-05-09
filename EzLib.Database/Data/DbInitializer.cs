using EzLib.Data;
using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Database.Data
{
    public static class DbInitializer
    {
        public static void Initialize(EzLibContext context)
        {
            context.Database.Migrate();

            if (!context.Category.Any())
            {
                var categories = new Category[]
                {
                    new Category { CategoryName = "Fiction" },
                    new Category { CategoryName = "Non-Fiction" },
                    new Category { CategoryName = "Science Fiction" },
                    new Category { CategoryName = "Biography" }
                };

                foreach (var category in categories)
                {
                    context.Category.Add(category);
                }

                context.SaveChanges();
            }

            if (!context.LibraryItem.Any())
            {
                var categories = context.Category.ToList();

                var items = new LibraryItem[]
                {
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
                    new LibraryItem
                    {
                        Title = "Sapiens: A Brief History of Humankind",
                        Author = "Yuval Noah Harari",
                        Pages = 464,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "Book",
                        Category = categories[1]
                    },
                    new LibraryItem
                    {
                        Title = "The Hitchhiker's Guide to the Galaxy",
                        Author = "Douglas Adams",
                        RunTimeMinutes = 109,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "Movie",
                        Category = categories[2]
                    },
                    new LibraryItem
                    {
                        Title = "The Matrix",
                        Author = "Lana Wachowski",
                        RunTimeMinutes = 136,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "Movie",
                        Category = categories[2]
                    },
                    new LibraryItem
                    {
                        Title = "The Social Network",
                        Author = "Aaron Sorkin",
                        RunTimeMinutes = 120,
                        IsBorrowable = true,
                        Borrower = "",
                        BorrowDate = null,
                        Type = "Movie",
                        Category = categories[3]
                    }
                };

                foreach (var item in items)
                {
                    context.LibraryItem.Add(item);
                }

                context.SaveChanges();
            }
        }
    }
}
