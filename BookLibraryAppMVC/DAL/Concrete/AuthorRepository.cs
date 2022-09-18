using BookLibraryAppMVC.DAL.Abstract;
using BookLibraryAppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryAppMVC.DAL.Concrete
{
    public class AuthorRepository : IAuthorRepository
    {
        private BookLibraryDBContext _db;
        public AuthorRepository(BookLibraryDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _db.Authors.ToListAsync();
        }

        public async Task<Author> GetById(int id)
        {
            return await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
