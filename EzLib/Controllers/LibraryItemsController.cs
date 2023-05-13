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
        public async Task<IActionResult> Index(string sortByType = null)
        {
            if (!string.IsNullOrEmpty(sortByType))
            {
                HttpContext.Session.SetString("sortByType", sortByType);
            }
            else if (HttpContext.Session.GetString("sortByType") != null)
            {
                sortByType = HttpContext.Session.GetString("sortByType");
            }

            var (libraryItems, updatedSortByType) = await _libraryItemsService.GetLibraryItemsAsync(sortByType);

            return View(libraryItems);
        }

        // GET: LibraryItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var libraryItem = await _libraryItemsService.GetLibraryItemDetailsAsync(id);

            if (libraryItem == null)
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        // GET: LibraryItems/Create     // No seperate service needed
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View();
        }

        // POST: LibraryItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Author,Pages,RunTimeMinutes,IsBorrowable,Type")] LibraryItem libraryItem)
        {
            var validationErrors = _libraryItemValidationService.ValidateLibraryItem(libraryItem);

            foreach (var error in validationErrors)
            {
                ModelState.Remove(error.Key);
            }

            if (ModelState.IsValid)
            {
                _context.Add(libraryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", libraryItem.CategoryId);
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

            // Check if the item is !borrowable and the borrower is !empty
            if (!libraryItem.IsBorrowable && !string.IsNullOrEmpty(libraryItem.Borrower))
            {
                return Forbid(); // Return forbidden status or redirect to an unauthorized page
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", libraryItem.CategoryId);
            return View(libraryItem);
        }


        // POST: LibraryItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Title,Author,Pages,RunTimeMinutes,IsBorrowable,Type")] LibraryItem libraryItem)
        {
            if (id != libraryItem.Id)
            {
                return NotFound();
            }

            var validationErrors = _libraryItemValidationService.ValidateLibraryItem(libraryItem);

            foreach (var error in validationErrors)
            {
                ModelState.Remove(error.Key);
            }

            if (ModelState.IsValid)
            {
                bool updateResult = await _libraryItemsService.UpdateLibraryItemAsync(id, libraryItem);

                if (!updateResult)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            _blockedFieldClearingService.ClearBlockedFields(libraryItem);

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

            // Check if the item is !borrowable and the borrower is !empty
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


        // Logic for borrowing an item

        // GET: LibraryItems/Borrow/5
        [HttpGet]
        public async Task<IActionResult> Borrow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryItem = await _borrowReturnLibraryItemService.GetBorrowableLibraryItemAsync(id.Value);

            if (libraryItem == null)
            {
                return NotFound();
            }

            string acronym = _acronymGeneratorService.GenerateAcronym(libraryItem.Title);
            libraryItem.Title = $"{acronym}";

            ViewBag.Borrower = new SelectList(_context.LibraryItem.Where(x => x.IsBorrowable && string.IsNullOrEmpty(x.Borrower)).Select(x => x.Borrower).Distinct());
            return View(libraryItem);
        }

        [HttpPost, ActionName("Borrow")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowConfirmed(int id, string borrower)
        {
            var result = await _borrowReturnLibraryItemService.BorrowConfirmed(id, borrower);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // Logic for returning an item

        // GET: LibraryItems/Return/5
        [HttpGet]
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryItem = await _borrowReturnLibraryItemService.GetReturnableLibraryItemAsync(id.Value);

            if (libraryItem == null || string.IsNullOrEmpty(libraryItem.Borrower))
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        [HttpPost, ActionName("Return")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfirmed(int id, string submit)
        {
            bool isConfirmed = await _borrowReturnLibraryItemService.ConfirmReturnAsync(id);

            if (!isConfirmed)
            {
                return NotFound();
            }

            if (submit == "yes")
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
