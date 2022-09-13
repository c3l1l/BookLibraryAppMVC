using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace BookLibraryAppMVC.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Column(TypeName = "varchar")]
        [DisplayName("Publisher Name")]
        public string PublisherName { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
