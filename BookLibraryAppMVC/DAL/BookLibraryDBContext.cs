using BookLibraryAppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryAppMVC.DAL
{
    public class BookLibraryDBContext:DbContext
    {
        public BookLibraryDBContext(DbContextOptions<BookLibraryDBContext> options):base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().HasData(
                new Author { Id=1,AuthorName="Robert C. Martin"},
                new Author { Id = 2, AuthorName = "Joseph Ingeno" },
                new Author { Id = 3, AuthorName = "Mark Richards and Neal Ford " }
                );

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id=1, PublisherName= "Pearson Education" },
                new Publisher { Id=2, PublisherName= "Packt Publishing" },
                new Publisher { Id=3, PublisherName= "O'Reilly Medya" }
                );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id=1, AuthorId=1, BookName="Clean Code", PageCount=431, PublisherId=1,DateReleased = new DateTime(2008,1,1)},
                new Book { Id=2, AuthorId=2, BookName= "Software Architect's Handbook", PageCount=594, PublisherId=2,DateReleased = new DateTime(2018,1,1)},
                new Book { Id=3, AuthorId=3, BookName= "Fundamentals of Software Architecture: An Engineering Approach", PageCount=396, PublisherId=3,DateReleased = new DateTime(2020,1,1)}
                );
        }
    }
}
