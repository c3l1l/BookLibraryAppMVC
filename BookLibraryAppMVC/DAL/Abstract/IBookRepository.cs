using BookLibraryAppMVC.Models;

namespace BookLibraryAppMVC.DAL.Abstract
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAll();
        Task<Book> GetById(int? id);
        Task Create(Book book);
        Task Update(Book book);
        Task Delete(Book book);
    }
}
