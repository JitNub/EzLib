using EzLib.Data;
using EzLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Controllers
{
    public class LibraryItemsController : Controller
    {
        private readonly EzLibContext _context;

        public LibraryItemsController(EzLibContext context)
        {
            _context = context;
        }

        // GET: LibraryItems
        public async Task<IActionResult> Index()
        {
            var ezLibContext = _context.LibraryItem.Include(l => l.Category);
            return View(await ezLibContext.ToListAsync());
        }

        // GET: LibraryItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LibraryItem == null)
            {
                return NotFound();
            }

            var libraryItem = await _context.LibraryItem
                .Include(l => l.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryItem == null)
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        // GET: LibraryItems/Create
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
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Author,Pages,RunTimeMinutes,IsBorrowable,Borrower,BorrowDate,Type")] LibraryItem libraryItem)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(libraryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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

            var libraryItem = await _context.LibraryItem.FindAsync(id);
            if (libraryItem == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", libraryItem.CategoryId);
            return View(libraryItem);
        }

        // POST: LibraryItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Title,Author,Pages,RunTimeMinutes,IsBorrowable,Borrower,BorrowDate,Type")] LibraryItem libraryItem)
        {
            if (id != libraryItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libraryItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryItemExists(libraryItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", libraryItem.CategoryId);
            return View(libraryItem);
        }

        // GET: LibraryItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LibraryItem == null)
            {
                return NotFound();
            }

            var libraryItem = await _context.LibraryItem
                .Include(l => l.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryItem == null)
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        // POST: LibraryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LibraryItem == null)
            {
                return Problem("Entity set 'EzLibContext.LibraryItem'  is null.");
            }
            var libraryItem = await _context.LibraryItem.FindAsync(id);
            if (libraryItem != null)
            {
                _context.LibraryItem.Remove(libraryItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryItemExists(int id)
        {
            return (_context.LibraryItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
