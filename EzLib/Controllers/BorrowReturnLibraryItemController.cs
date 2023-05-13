using EzLib.Data;
using EzLib.Models;
using EzLib.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EzLib.Controllers
{
    public class BorrowReturnLibraryItemController : Controller
    {
        private readonly EzLibContext _context;
        private readonly IAcronymGeneratorService _acronymGeneratorService;
        private readonly ILibraryItemsService _libraryItemsService;
        private readonly IBorrowReturnLibraryItemService _borrowReturnLibraryItemService;
        

        public BorrowReturnLibraryItemController(EzLibContext context, IAcronymGeneratorService acronymGeneratorService, ILibraryItemsService libraryItemsService, IBorrowReturnLibraryItemService borrowReturnLibraryItemService)
        {
            _context = context;
            _acronymGeneratorService = acronymGeneratorService;
            _libraryItemsService = libraryItemsService;
            _borrowReturnLibraryItemService = borrowReturnLibraryItemService;
        }

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

            // Populate any necessary data for the view (e.g., ViewBag)

            return View(libraryItem);
        }

        [HttpPost, ActionName("Borrow")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowConfirmed(int id, string borrower)
        {
            // Create a temporary LibraryItem object to validate the borrower string
            var tempLibraryItem = new LibraryItem { Borrower = borrower };

            // Validate the Borrower property using Data Annotations
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(tempLibraryItem) { MemberName = nameof(tempLibraryItem.Borrower) };
            Validator.TryValidateProperty(tempLibraryItem.Borrower, validationContext, validationResults);

            // Add validation errors to the ModelState
            foreach (var validationResult in validationResults)
            {
                ModelState.AddModelError(nameof(borrower), validationResult.ErrorMessage);
            }

            if (ModelState.IsValid)
            {
                var result = await _borrowReturnLibraryItemService.BorrowConfirmed(id, borrower);

                if (!result)
                {
                    return NotFound();
                }

                // Handle the redirection or view rendering after successful borrowing
                return RedirectToAction("Details", "LibraryItems", new { id = id });
            }
            else
            {
                // Populate the necessary data for the view and return the Borrow view with the current model
                var libraryItem = await _borrowReturnLibraryItemService.GetBorrowableLibraryItemAsync(id);
                if (libraryItem == null)
                {
                    return NotFound();
                }
                return View("Borrow", libraryItem);
            }
        }


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

            // Populate any necessary data for the view (e.g., ViewBag)

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
                return RedirectToAction("Details", "LibraryItems", new { id = id });
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
