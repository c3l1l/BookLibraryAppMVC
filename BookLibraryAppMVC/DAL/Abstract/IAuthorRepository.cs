using BookLibraryAppMVC.Models;

namespace BookLibraryAppMVC.DAL.Abstract
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAll();
        Task<Author> GetById(int id);
        //Task Create(Book book);
        //void Update(Book book);
        //void Delete(Book book);
    }
}
