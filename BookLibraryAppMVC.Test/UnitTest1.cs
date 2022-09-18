using BookLibraryAppMVC.Controllers;
using BookLibraryAppMVC.DAL.Abstract;
using BookLibraryAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
            _controller = new BooksController(_mockBookRepo.Object,_mockAuthorRepo.Object,_mockPublisherRepo.Object);
        }
        [Fact]
        public async void Index_ActionExecute_ReturnView()
        {
            var result= await _controller.Index();
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void Index_ActionExecute_ReturnBookList()
        {                        
            var seedData = GetBookData();
            _mockBookRepo.Setup(repo => repo.GetAll()).ReturnsAsync(seedData);
            var result = await _controller.Index();            
            
            var viewResult=Assert.IsType<ViewResult>(result);
            var bookList = Assert.IsAssignableFrom<IEnumerable<Book>>(viewResult.Model);
            Assert.Equal<int>(4, bookList.ToList().Count());
        }
        [Fact]
        public async void Details_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = await _controller.Details(null);
            var redirect=Assert.IsType<RedirectToActionResult>(result);
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
            var notFoundResult=Assert.IsType<NotFoundResult>(result);
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