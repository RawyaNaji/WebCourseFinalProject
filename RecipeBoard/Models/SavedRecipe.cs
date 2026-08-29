using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RecipeBoard.Models
{
    public class SavedRecipe
    {
        public int SavedRecipeId { get; set; }

        public int UserId { get; set; }
        [ValidateNever]
        public User User { get; set; }

        public int RecipeId { get; set; }
        [ValidateNever]
        public Recipe Recipe { get; set; }

        public DateTime SavedDate { get; set; }
    }
}
