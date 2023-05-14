using EzLib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EzLib.Data
{
    public class DbInitializer : IDbInitializer
    {
        // Declare private fields
        private readonly EzLibContext _context;
        private readonly ILogger<DbInitializer> _logger;

        // Constructor with dependencies injected via DI
        public DbInitializer(EzLibContext context, ILogger<DbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        // InitializeAsync method for database initialization
        public async Task InitializeAsync()
        {
            try
            {
                // Check if the database exists, if not create it
                _logger.LogInformation("Creating the database if it doesn't exist...");
                await _context.Database.EnsureCreatedAsync();
                _logger.LogInformation("Database created or already exists.");

                // Apply any pending database migrations
                _logger.LogInformation("Applying database migrations...");
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                // Log any errors that occurred during initialization
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
    }
}
