using BookLibraryAppMVC.DAL.Abstract;
using BookLibraryAppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryAppMVC.DAL.Concrete
{
    public class BookRepository : IBookRepository
    {
        private BookLibraryDBContext _db;
        public BookRepository(BookLibraryDBContext db)
        {
            _db=db;
        }
        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _db.Books.Include(b=>b.Author).Include(b=>b.Publisher).ToListAsync();
        }
        public async Task<Book> GetById(int? id)
        {
            //var book=_db.Books.Find(id);
            return await _db.Books.Include(b=>b.Author).Include(b=>b.Publisher).FirstOrDefaultAsync(b=>b.Id==id);
        }
        public async Task Create(Book book)
        {
             await _db.AddAsync(book);            
             await _db.SaveChangesAsync();            
        }
      
        public  async Task Update(Book book)
        {
              _db.Update(book);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(Book book)
        {
            _db.Remove(book);
            await _db.SaveChangesAsync();
        }
    }
}
