using EzLib.Models;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Data
{
    public class EzLibContext : DbContext
    {
        public EzLibContext(DbContextOptions<EzLibContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Employees> Employees { get; set; }

        public DbSet<LibraryItem> LibraryItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.LibraryItems)
                .WithOne(l => l.Category)
                .HasForeignKey(l => l.CategoryId);
        }
    }
}
