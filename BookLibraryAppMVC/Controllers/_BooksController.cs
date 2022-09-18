using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookLibraryAppMVC.DAL;
using BookLibraryAppMVC.Models;
using BookLibraryAppMVC.DAL.Abstract;

namespace BookLibraryAppMVC.Controllers
{
    public class _BooksController : Controller
    {
         private readonly BookLibraryDBContext _context;
       

        public _BooksController(BookLibraryDBContext context)
      
        {
            //_context = context;
           
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var bookLibraryDBContext = _context.Books.Include(b => b.Author).Include(b => b.Publisher);
            return View(await bookLibraryDBContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "AuthorName");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookName,DateReleased,AuthorId,PublisherId,PageCount")] BookVM book)
        {
            if (ModelState.IsValid)
            {
                Book _book = new Book();
                _book.BookName = book.BookName;
                _book.AuthorId = book.AuthorId;
                _book.PublisherId = book.PublisherId;
                _book.DateReleased = book.DateReleased;
                _book.PageCount = book.PageCount;
                _context.Add(_book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", book.PublisherId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "AuthorName", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookName,DateReleased,AuthorId,PublisherId,PageCount")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "AuthorName", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookLibraryDBContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
