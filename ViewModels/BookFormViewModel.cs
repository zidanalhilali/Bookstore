using Bookstore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;


        [StringLength(2000)]
        public string? Description { get; set; }


        [Display(Name = "Author")]
        [Required(ErrorMessage = "Please select an author")]
        public int? AuthorId { get; set; }


        [Display(Name = "Cover Image")]
        public IFormFile? File { get; set; }
        public IEnumerable<Author>? Authors { get; set; }
        public string? ImageUrl { get; set; }
    }
}
