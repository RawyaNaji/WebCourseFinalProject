using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RecipeBoard.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Instructions are required.")]
        [StringLength(4000)]
        public string Instructions { get; set; }

        [Range(1, 500, ErrorMessage = "Prep time must be between 1 and 500 minutes.")]
        public int PrepTimeMinutes { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
    }
}
