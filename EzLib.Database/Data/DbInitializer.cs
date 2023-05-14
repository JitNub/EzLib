using EzLib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EzLib.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly EzLibContext _context;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(EzLibContext context, ILogger<DbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Creating the database if it doesn't exist...");
                await _context.Database.EnsureCreatedAsync();
                _logger.LogInformation("Database created or already exists.");

                _logger.LogInformation("Applying database migrations...");
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Database migrations applied successfully.");

                // Perform additional initialization logic here if needed
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
    }
}
