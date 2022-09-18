using BookLibraryAppMVC.Models;

namespace BookLibraryAppMVC.DAL.Abstract
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<Publisher>> GetAll();
        Task<Publisher> GetById(int id);
        //Task Create(Book book);
        //void Update(Book book);
        //void Delete(Book book);
    }
}
