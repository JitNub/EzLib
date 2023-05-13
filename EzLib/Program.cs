using EzLib.Data;
using EzLib.Database.Data;
using EzLib.Services.Services;
using EzLib.Web.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IAcronymGeneratorService, AcronymGeneratorService>(); // Generate Acronym
builder.Services.AddScoped<ICategoryService, CategoryService>();  // Validation of Category
builder.Services.AddScoped<ILibraryItemsService, LibraryItemsService>();
builder.Services.AddScoped<IBorrowReturnLibraryItemService, BorrowReturnLibraryItemService>();
builder.Services.AddScoped<ILibraryItemValidationService, LibraryItemValidationService>();
builder.Services.AddScoped<IBlockedFieldClearingService, BlockedFieldClearingService>();

builder.Services.AddDbContext<EzLibContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EzLibContext") ?? throw new InvalidOperationException("Connection string 'EzLibContext' not found.")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<EzLibContext>();

    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();   // Use session

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
