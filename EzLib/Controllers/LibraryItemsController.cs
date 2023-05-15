using EzLib.Data;
using EzLib.Models;
using EzLib.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EzLib.Controllers
{
    public class LibraryItemsController : Controller
    {
        private readonly EzLibContext _context;
        private readonly IAcronymGeneratorService _acronymGeneratorService;
        private readonly ILibraryItemsService _libraryItemsService;
        private readonly IBorrowReturnLibraryItemService _borrowReturnLibraryItemService;
        private readonly ILibraryItemValidationService _libraryItemValidationService;
        private readonly IBlockedFieldClearingService _blockedFieldClearingService;

        public LibraryItemsController(EzLibContext context, IAcronymGeneratorService acronymGeneratorService, ILibraryItemsService libraryItemsService, IBorrowReturnLibraryItemService borrowReturnLibraryItemService, ILibraryItemValidationService libraryItemValidationService,
            IBlockedFieldClearingService blockedFieldClearingService)
        {
            _context = context;
            _acronymGeneratorService = acronymGeneratorService;
            _libraryItemsService = libraryItemsService;
            _borrowReturnLibraryItemService = borrowReturnLibraryItemService;
            _libraryItemValidationService = libraryItemValidationService;
            _blockedFieldClearingService = blockedFieldClearingService;
        }

        // GET: LibraryItems
        public async Task<IActionResult> Index(string sortByType = null, string searchString = null)
        {
            // Check if sortByType parameter is provided
            if (!string.IsNullOrEmpty(sortByType))
            {
                // Store sortByType value in session
                HttpContext.Session.SetString("sortByType", sortByType);
            }
            else if (HttpContext.Session.GetString("sortByType") != null)
            {
                // Retrieve sortByType value from session
                sortByType = HttpContext.Session.GetString("sortByType");
            }

            // Get library items based on sortByType and searchString
            var (libraryItems, updatedSortByType) = await _libraryItemsService.GetLibraryItemsAsync(sortByType, searchString);

            return View(libraryItems);
        }

        // GET: LibraryItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Retrieve library item details by ID
            var libraryItem = await _libraryItemsService.GetLibraryItemDetailsAsync(id);

            if (libraryItem == null)
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        // GET: LibraryItems/Create
        public IActionResult Create()
        {
            // Populate the CategoryId dropdown list
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View();
        }

        // POST: LibraryItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Author,Pages,RunTimeMinutes,IsBorrowable,Type")] LibraryItem libraryItem)
        {
            // Validate the library item
            var validationErrors = _libraryItemValidationService.ValidateLibraryItem(libraryItem);

            // Remove validation errors from ModelState
            foreach (var error in validationErrors)
            {
                ModelState.Remove(error.Key);
            }

            if (ModelState.IsValid)
            {
                // Check if the library item title is unique
                if (!await _libraryItemsService.IsLibraryItemTitleUnique(libraryItem))
                {
                    ModelState.AddModelError(string.Empty, "Title name must be unique.");
                    ViewBag.CategoryId = new SelectList(_context.Category, "Id", "CategoryName", libraryItem.CategoryId);
                    return View(libraryItem);
                }

                // Add the library item and save changes
                _context.Add(libraryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Populate the CategoryId dropdown list
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", libraryItem.CategoryId);
            return View(libraryItem);
        }


        // GET: LibraryItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LibraryItem == null)
            {
                return NotFound();
            }

            var libraryItem = await _libraryItemsService.GetLibraryItemAsync(id);
            if (libraryItem == null)
            {
                return NotFound();
            }

            // Check if the item is not borrowable and the borrower is not empty
            if (!libraryItem.IsBorrowable && !string.IsNullOrEmpty(libraryItem.Borrower))
            {
                return Forbid(); // Return forbidden status or redirect to an unauthorized page
            }

            // Populate the CategoryId dropdown list
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View(libraryItem);
        }

        // POST: LibraryItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Title,Author,Pages,RunTimeMinutes,IsBorrowable,Type")] LibraryItem libraryItem)
        {
            if (id != libraryItem.Id)
            {
                return NotFound();
            }

            var validationErrors = _libraryItemValidationService.ValidateLibraryItem(libraryItem);

            // Remove validation errors from ModelState
            foreach (var error in validationErrors)
            {
                ModelState.Remove(error.Key);
            }

            if (ModelState.IsValid)
            {
                // Check if the library item title is unique
                if (!await _libraryItemsService.IsLibraryItemTitleUnique(libraryItem))
                {
                    ModelState.AddModelError("UniqueTitle", "Title name must be unique.");
                    // Populate the SelectList for the Category dropdown before returning the View
                    ViewBag.CategoryId = new SelectList(_context.Category, "Id", "CategoryName", libraryItem.CategoryId);
                    return View(libraryItem);
                }

                // Update the library item
                bool updateResult = await _libraryItemsService.UpdateLibraryItemAsync(id, libraryItem);

                if (!updateResult)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            // Populate the CategoryId dropdown list
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", libraryItem.CategoryId);
            return View(libraryItem);
        }

        // GET: LibraryItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var libraryItem = await _libraryItemsService.GetLibraryItemForDeleteAsync(id);

            if (libraryItem == null)
            {
                return NotFound();
            }

            // Check if the item is not borrowable and the borrower is not empty
            if (!libraryItem.IsBorrowable && !string.IsNullOrEmpty(libraryItem.Borrower))
            {
                return Forbid(); // Return forbidden status or redirect to an unauthorized page
            }

            return View(libraryItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await _libraryItemsService.DeleteLibraryItemAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
