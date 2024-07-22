using System.ComponentModel.DataAnnotations;

namespace StudyPersonalLibrary.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
    }
}
