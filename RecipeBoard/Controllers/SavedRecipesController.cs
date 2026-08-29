using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBoard.Data;
using RecipeBoard.Models;

namespace RecipeBoard.Controllers
{
    public class SavedRecipesController : Controller
    {
        private readonly ApplicationDbContext context;

        public SavedRecipesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: /SavedRecipes  (My Saved Recipes)
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var savedRecipes = context.SavedRecipes
                .Include(s => s.Recipe)
                .ThenInclude(r => r.Category)
                .Where(s => s.UserId == userId.Value)
                .OrderByDescending(s => s.SavedDate)
                .ToList();

            return View(savedRecipes);
        }

        // POST: /SavedRecipes/Save/5
        [HttpPost]
        public IActionResult Save(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            bool alreadySaved = context.SavedRecipes
                .Any(s => s.UserId == userId.Value && s.RecipeId == recipeId);

            if (!alreadySaved)
            {
                context.SavedRecipes.Add(new SavedRecipe
                {
                    UserId = userId.Value,
                    RecipeId = recipeId,
                    SavedDate = DateTime.Now
                });
                context.SaveChanges();
            }

            return RedirectToAction("Details", "Recipes", new { id = recipeId });
        }

        // POST: /SavedRecipes/Remove/5
        [HttpPost]
        public IActionResult Remove(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            SavedRecipe savedRecipe = context.SavedRecipes
                .FirstOrDefault(s => s.UserId == userId.Value && s.RecipeId == recipeId);

            if (savedRecipe != null)
            {
                context.SavedRecipes.Remove(savedRecipe);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
