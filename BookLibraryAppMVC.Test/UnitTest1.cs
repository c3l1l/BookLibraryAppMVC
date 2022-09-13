using BookLibraryAppMVC.Controllers;
using BookLibraryAppMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookLibraryAppMVC.Test
{
    public class UnitTest1
    {
        private readonly Mock<BookLibraryDBContext> _mockRepo;
        private readonly BooksController _controller;
        private readonly Mock<BooksController> _controllerMock = new Mock<BooksController>(new Mock<BookLibraryDBContext>().Object);
       
        public UnitTest1()
        {
            _mockRepo=new Mock<BookLibraryDBContext>();
            _controller=new BooksController(_mockRepo.Object);
            
        }
      
        [Fact]
        public void Test1()
        {
          
            var result = _controllerMock.Index();
            
            Assert.IsType<ViewResult>(result);
            //Assert.NotNull(result);

        }
    }
}