using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? ImageUrl { get; set; }

        public int AuthorId { get; set; }   // Foreign Key
        public Author Author { get; set; } = null!;
    }
}