using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Dtos.Book
{
    public class BookAddDto
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string Author { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(350, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public DateTime PublishDate { get; set; }
    }
}