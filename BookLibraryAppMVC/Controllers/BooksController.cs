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
using NuGet.Protocol;

namespace BookLibraryAppMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IPublisherRepository _publisherRepository;

        public BooksController(IBookRepository bookRepository,IAuthorRepository authorRepository,IPublisherRepository publisherRepository)
        {           
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {                                 
                       
            return View(await _bookRepository.GetAll());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
           return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {           
            await GetAuthorAndPuslisherSelectList();
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
               await _bookRepository.Create(_book);              
                return RedirectToAction(nameof(Index));
            }

            await GetAuthorAndPuslisherSelectList();
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null ||await _bookRepository.GetAll() == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            await GetAuthorAndPuslisherSelectList();
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
                 await _bookRepository.Update(book);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
           await GetAuthorAndPuslisherSelectList();
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || await _bookRepository.GetAll() == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetById(id);
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
            if (await _bookRepository.GetAll() == null)
            {
                return Problem("Entity set 'BookLibraryDBContext.Books'  is null.");
            }
            var book = await _bookRepository.GetById(id);
            if (book != null)
            {
             await _bookRepository.Delete(book);
            }

           
            return RedirectToAction(nameof(Index));
        }

        private async Task GetAuthorAndPuslisherSelectList()
        {
            ViewData["AuthorId"] = new SelectList(await _authorRepository.GetAll(), "Id", "AuthorName");
            ViewData["PublisherId"] = new SelectList(await _publisherRepository.GetAll(), "Id", "PublisherName");
        }
    }
}
