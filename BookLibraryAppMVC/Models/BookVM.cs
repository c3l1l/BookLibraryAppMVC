using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace BookLibraryAppMVC.Models
{
    public class BookVM
    {     
       
        [DisplayName("Book Name")]
        public string BookName { get; set; }
        [DisplayName("Released Date")]
        public DateTime? DateReleased { get; set; }
        [DisplayName("Author")]
        public int AuthorId { get; set; }
        [DisplayName("Publisher")]
        public int PublisherId { get; set; }
        [DisplayName("Page Count")]
        public int PageCount { get; set; }

      
    }
}
