using BookLibraryAppMVC.Controllers;
using BookLibraryAppMVC.DAL.Abstract;
using BookLibraryAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace BookLibraryAppMVC.Test
{
    public class BooksControllerTest
    {
        private readonly Mock<IBookRepository> _mockBookRepo;
        private readonly Mock<IAuthorRepository> _mockAuthorRepo;
        private readonly Mock<IPublisherRepository> _mockPublisherRepo;
        private readonly BooksController _controller;

        public BooksControllerTest()
        {
            _mockBookRepo = new Mock<IBookRepository>();
            _mockAuthorRepo = new Mock<IAuthorRepository>();
            _mockPublisherRepo = new Mock<IPublisherRepository>();
            _controller = new BooksController(_mockBookRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object);
        }
        [Fact]
        public async void Index_ActionExecute_ReturnView()
        {
            var result = await _controller.Index();
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void Index_ActionExecute_ReturnBookList()
        {
            var seedData = GetBookData();
            _mockBookRepo.Setup(repo => repo.GetAll()).ReturnsAsync(seedData);
            var result = await _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var bookList = Assert.IsAssignableFrom<IEnumerable<Book>>(viewResult.Model);
            Assert.Equal<int>(4, bookList.ToList().Count());
        }
        [Fact]
        public async void Details_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = await _controller.Details(null);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        [Theory]
        [InlineData(3)]
        public async void Details_IdInValid_ReturnNotFound(int bookId)
        {
            //Book book = new Book() { Id = 1, BookName = "Book Test 1", PageCount = 11, PublisherId = 1, AuthorId = 1, DateReleased = new DateTime(2000, 1, 1) };
            Book book = null;
            _mockBookRepo.Setup(repo => repo.GetById(bookId)).ReturnsAsync(book);
            var result = await _controller.Details(bookId);
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, notFoundResult.StatusCode);
        }
        [Theory]
        [InlineData(3)]
        public async void Details_IdValid_ReturnBook(int bookId)
        {

            Book book = GetBookData().First(x => x.Id == bookId);
            _mockBookRepo.Setup(repo => repo.GetById(bookId)).ReturnsAsync(book);
            var result = await _controller.Details(bookId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var resultBook = Assert.IsAssignableFrom<Book>(viewResult.Model);

            Assert.Equal<int>(book.Id, resultBook.Id);
            Assert.Equal(book.BookName, resultBook.BookName);
        }
        [Fact]
        public async void Create_ActionExecute_ReturnView()
        {
            var result = await _controller.Create();
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void CreatePOST_InValidModelState_ReturnView()
        {
            BookVM bookVm = new BookVM() { AuthorId = 1, PageCount = 12 };

            _controller.ModelState.AddModelError("BookName", "Name field is required.");
            var result = await _controller.Create(bookVm);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<BookVM>(viewResult.Model);
        }
        [Fact]
        public async void CreatePOST_ValidModelState_ReturnRedirectToActionIndex()
        {
            BookVM bookVm = new BookVM() { AuthorId = 1, BookName = "Test", DateReleased = new DateTime(2000, 1, 1), PublisherId = 1, PageCount = 12 };
            var result = await _controller.Create(bookVm);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

        }
        [Fact]
        public async void CreatePOST_ValidModelState_CreateMethodExecute()
        {
            Book newBook = null;
            BookVM bookVm = new BookVM() { AuthorId = 1, BookName = "Test", DateReleased = new DateTime(2000, 1, 1), PublisherId = 1, PageCount = 12 };
            _mockBookRepo.Setup(repo => repo.Create(It.IsAny<Book>())).Callback<Book>(x => newBook = x);
            var result = await _controller.Create(bookVm);
            _mockBookRepo.Verify(repo => repo.Create(It.IsAny<Book>()), Times.Once);
            Assert.Equal(bookVm.BookName, newBook.BookName);
        }
        [Fact]
        public async void CreatePOST_InValidModelState_NeverCreateExecute()
        {
            BookVM bookVm = new BookVM() { AuthorId = 1, BookName = "Test", DateReleased = new DateTime(2000, 1, 1), PublisherId = 1, PageCount = 12 };
            _controller.ModelState.AddModelError("BookName", "");
            var result = await _controller.Create(bookVm);
            _mockBookRepo.Verify(repo => repo.Create(It.IsAny<Book>()), Times.Never);
        }
        [Fact]
        public async void Edit_IdIsNull_ReturnNotFound()
        {
            var result = await _controller.Edit(null);
            var notFound = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }

        [Theory]
        [InlineData(3)]
        public async void Edit_IdInValid_ReturnNotFound(int id)
        {
            // Book book = GetBookData().First(x => x.Id == id);
            Book book = null;
            _mockBookRepo.Setup(repo => repo.GetById(id)).ReturnsAsync(book);
            var result = await _controller.Edit(id);
            var notFound = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }
        [Theory]
        [InlineData(2)]
        public async void Edit_ActionExecute_ReturnBook(int bookId)
        {
            Book book = GetBookData().First(x => x.Id == bookId);
            _mockBookRepo.Setup(repo => repo.GetById(bookId)).ReturnsAsync(book);
            var result = await _controller.Edit(bookId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var resultBook = Assert.IsAssignableFrom<Book>(viewResult.Model);
            Assert.Equal(bookId, resultBook.Id);
            Assert.Equal(book.BookName, resultBook.BookName);

        }
        [Theory]
        [InlineData(1)]
        public async void EditPOST_IdIsNotEqualBook_ReturnNotFound(int bookId)
        {
            var result = await _controller.Edit(3, GetBookData().First(x => x.Id == bookId));
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void EditPOST_InValidModelState_ReturnView(int bookId)
        {
            _controller.ModelState.AddModelError("BookName", "");
            var result = await _controller.Edit(bookId, GetBookData().First(x => x.Id == bookId));
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Book>(viewResult.Model);
        }
        [Theory]
        [InlineData(1)]
        public async void EditPOST_ValidModelState_ReturnRedirectToIndex(int bookId)
        {
            var result = await _controller.Edit(bookId, GetBookData().First(x => x.Id == bookId));
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        [Theory]
        [InlineData(1)]
        public async void EditPOST_ValidModelState_UpdateMethodExecute(int bookId)
        {
            var book = GetBookData().First(x => x.Id == bookId);
            _mockBookRepo.Setup(repo => repo.Update(book));
            await _controller.Edit(bookId, book);
            _mockBookRepo.Verify(repo => repo.Update(It.IsAny<Book>()), Times.Once);

        }
        [Fact]
        public async void Delete_IdIsNull_ReturnNotFound()
        {
            var result = await _controller.Delete(null);
            Assert.IsType<NotFoundResult>(result);
        }
        [Theory]
        [InlineData(1)]
        public async void Delete_IdIsNotEqualBook_ReturnNotFound(int bookId)
        {
            Book book = null;
            _mockBookRepo.Setup(repo => repo.GetById(bookId)).ReturnsAsync(book);
            var result = await _controller.Delete(bookId);
            Assert.IsType<NotFoundResult>(result);
        }
        [Theory]
        [InlineData(3)]
        public async void Delete_ActionExecute_ReturnBook(int bookId)
        {
            Book book = GetBookData().First(b=>b.Id==bookId);
            _mockBookRepo.Setup(repo => repo.GetById(bookId)).ReturnsAsync(book);
            var result = await _controller.Delete(bookId);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Book>(viewResult.Model);
        }
        [Theory]
        [InlineData(3)]
        public async void DeleteConfirmed_ActionExecute_ReturnRedirectToActionIndex(int bookId)
        { 
            var result=await _controller.DeleteConfirmed(bookId);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecute_DeleteMethodExecute(int bookId)
        {
            var book = GetBookData().First(b => b.Id == bookId);
            _mockBookRepo.Setup(repo => repo.Delete(book));
            var result=await _controller.DeleteConfirmed(bookId);
           _mockBookRepo.Verify(repo => repo.Delete(It.IsAny<Book>()),Times.Once);  
        }

        private  IEnumerable<Book> GetBookData()
        {
            List<Book> bookData = new List<Book>()
            {
                new Book(){Id=1, BookName="Book Test 1", PageCount=11, PublisherId=1, AuthorId=1, DateReleased = new DateTime(2000,1,1)},
                new Book(){Id=2, BookName="Book Test 2", PageCount=22, PublisherId=2, AuthorId=2, DateReleased = new DateTime(2005,1,1)},
                new Book(){Id=3, BookName="Book Test 3", PageCount=33, PublisherId=3, AuthorId=3, DateReleased = new DateTime(2010,1,1)},
                new Book(){Id=4, BookName="Book Test 4", PageCount=44, PublisherId=1, AuthorId=1, DateReleased = new DateTime(2015,1,1)},
            };

            return bookData;
        }
    }
}