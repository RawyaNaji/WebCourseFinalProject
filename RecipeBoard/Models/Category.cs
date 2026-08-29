using System.ComponentModel.DataAnnotations;

namespace RecipeBoard.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
