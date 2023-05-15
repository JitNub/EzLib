using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Data
{
    public class EzLibContext : DbContext
    {
        // Constructor that accepts DbContextOptions<EzLibContext> for database configuration
        public EzLibContext(DbContextOptions<EzLibContext> options) : base(options)
        {
        }

        // DbSet for the Category model
        public DbSet<Category> Category { get; set; }

        // DbSet for the Employees model
        public DbSet<Employees> Employees { get; set; }

        // DbSet for the LibraryItem model
        public DbSet<LibraryItem> LibraryItem { get; set; }

        // Method for configuring the model relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.LibraryItems) // Category has many LibraryItems
                .WithOne(l => l.Category) // LibraryItem has one Category
                .HasForeignKey(l => l.CategoryId); // LibraryItem's foreign key is CategoryId
        }
    }
}
