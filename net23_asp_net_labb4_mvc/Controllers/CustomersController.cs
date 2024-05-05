using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net23_asp_net_labb4_mvc.Data;
using net23_asp_net_labb4_mvc.Models;

namespace net23_asp_net_labb4_mvc.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers
                .Include(c => c.BorrowedBooks)
                .ThenInclude(bb => bb.Book)
                .ToListAsync();

            return View(customers);
        }
        // GET: Customers/Search
        [HttpGet]
        public IActionResult Search()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            return View();
        }
        // POST: Customers/Search
        [HttpPost]
        public IActionResult Search(int? customerId)
        {
            if (customerId == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = customerId });
        }
        // GET: Customers/BorrowBook
        [HttpGet]
        public async Task<IActionResult> BorrowBook()
        {
            var customers = await _context.Customers.ToListAsync();
            var books = await _context.Books.Where(b => b.AmountInStock > 0).ToListAsync();

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["BookId"] = new SelectList(_context.Books.Where(b => b.AmountInStock > 0), "Id", "Title");

            return View();
        }
        // POST: Customers/BorrowBook
        [HttpPost]
        public async Task<IActionResult> BorrowBook(int? customerId, int? bookId)
        {
            if (customerId == null || bookId == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == bookId && b.AmountInStock > 0);

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (book == null || customer == null)
            {
                return NotFound();
            }

            var borrowedBook = new BorrowedBook
            {
                CustomerId = customer.Id,
                BookId = book.Id,
                BorrowedDate = DateTime.Now
            };

            _context.BorrowedBooks.Add(borrowedBook);
            book.AmountInStock--;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = customerId });
        }
        // POST: Customers/ReturnBook
        [HttpGet]
        public async Task<IActionResult> ReturnBook(int? customerId, int? bookId)
        {
            if (customerId == null || bookId == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);

            var borrowedBook = await _context.BorrowedBooks.FirstOrDefaultAsync(bb => bb.BookId == bookId && bb.CustomerId == customerId);

            if (book == null || borrowedBook == null)
            {
                return NotFound();
            }

            _context.BorrowedBooks.Remove(borrowedBook);
            book.AmountInStock++;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = customerId });
        }
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.BorrowedBooks)
                .ThenInclude(bb => bb.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
