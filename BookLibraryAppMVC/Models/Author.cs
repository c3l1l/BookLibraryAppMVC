global using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibraryAppMVC.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Column(TypeName ="varchar")]
        [DisplayName("Author Name")]
        public string AuthorName { get; set; }

        public ICollection<Book> Books { get; set; }


    }
}
