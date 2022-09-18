using BookLibraryAppMVC.DAL.Abstract;
using BookLibraryAppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryAppMVC.DAL.Concrete
{
    public class PublisherRepository : IPublisherRepository
    {
        private BookLibraryDBContext _db;
        public PublisherRepository(BookLibraryDBContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Publisher>> GetAll()
        {
            return await _db.Publishers.ToListAsync();
        }

        public async Task<Publisher> GetById(int id)
        {
            return await _db.Publishers.SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
