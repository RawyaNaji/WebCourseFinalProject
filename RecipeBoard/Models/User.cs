using System.ComponentModel.DataAnnotations;

namespace RecipeBoard.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "Admin" or "User"
    }
}
