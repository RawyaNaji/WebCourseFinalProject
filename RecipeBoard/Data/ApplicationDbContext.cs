using Microsoft.EntityFrameworkCore;
using RecipeBoard.Models;

namespace RecipeBoard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Breakfast" },
                new Category { CategoryId = 2, Name = "Lunch" },
                new Category { CategoryId = 3, Name = "Dinner" },
                new Category { CategoryId = 4, Name = "Dessert" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "admin", Password = "admin123", Role = "Admin" },
                new User { UserId = 2, Username = "guest", Password = "guest123", Role = "User" }
            );

            modelBuilder.Entity<Recipe>().HasData(
                new Recipe { RecipeId = 1, Title = "Pancakes", Instructions = "Mix flour, milk, and eggs. Cook on a griddle until golden on both sides.", PrepTimeMinutes = 20, CategoryId = 1 },
                new Recipe { RecipeId = 2, Title = "Grilled Cheese Sandwich", Instructions = "Butter two slices of bread, add cheese, and grill until golden.", PrepTimeMinutes = 10, CategoryId = 2 },
                new Recipe { RecipeId = 3, Title = "Spaghetti Bolognese", Instructions = "Cook pasta. Brown ground beef with onions and garlic, add tomato sauce, simmer, and combine.", PrepTimeMinutes = 40, CategoryId = 3 },
                new Recipe { RecipeId = 4, Title = "Chocolate Chip Cookies", Instructions = "Cream butter and sugar, add eggs and vanilla, mix in flour and chocolate chips, bake at 350F for 10 minutes.", PrepTimeMinutes = 30, CategoryId = 4 }
            );
        }
    }
}
