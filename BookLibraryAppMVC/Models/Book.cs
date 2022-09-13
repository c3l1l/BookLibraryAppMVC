using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibraryAppMVC.Models
{
    public class Book
    {
        [Key]
       
        public int Id { get; set; }
        [StringLength (100)]
        [Column (TypeName = "varchar")]
        [DisplayName ("Book Name")]
        public string BookName { get; set; }
        [DisplayName("Released Date")]
        public DateTime? DateReleased { get; set; }
        [ForeignKey("Author")]
        [DisplayName("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("Publisher")]
        [DisplayName("Publisher")]
        public int PublisherId { get; set; }
        public int PageCount { get; set; }

        public Author? Author { get; set; }
        public Publisher? Publisher { get; set; }


    }
}


