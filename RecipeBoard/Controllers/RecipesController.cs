using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBoard.Data;
using RecipeBoard.Models;

namespace RecipeBoard.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext context;

        public RecipesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // GET: /Recipes
        public IActionResult Index(string searchString, int? categoryId)
        {
            var recipes = context.Recipes
                .Include(r => r.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                recipes = recipes.Where(r => r.Title.Contains(searchString));
            }

            if (categoryId.HasValue)
            {
                recipes = recipes.Where(r => r.CategoryId == categoryId.Value);
            }

            ViewBag.Categories = context.Categories.OrderBy(c => c.Name).ToList();
            ViewBag.SearchString = searchString;
            ViewBag.CategoryId = categoryId;

            return View(recipes.OrderBy(r => r.Title).ToList());
        }

        // GET: /Recipes/Details/5
        public IActionResult Details(int id)
        {
            Recipe recipe = context.Recipes
                .Include(r => r.Category)
                .FirstOrDefault(r => r.RecipeId == id);

            if (recipe == null)
            {
                return NotFound();
            }

            // Session: remember the most recently viewed recipe and a running view count
            HttpContext.Session.SetString("LastViewedRecipeTitle", recipe.Title);
            HttpContext.Session.SetInt32("LastViewedRecipeId", recipe.RecipeId);

            int viewCount = HttpContext.Session.GetInt32("ViewCount") ?? 0;
            HttpContext.Session.SetInt32("ViewCount", viewCount + 1);

            int? userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsSaved = userId != null &&
                context.SavedRecipes.Any(s => s.UserId == userId.Value && s.RecipeId == id);

            return View(recipe);
        }

        // GET: /Recipes/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Categories = context.Categories.OrderBy(c => c.Name).ToList();
            return View();
        }

        // POST: /Recipes/Create
        [HttpPost]
        public IActionResult Create(Recipe recipe)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = context.Categories.OrderBy(c => c.Name).ToList();
                return View(recipe);
            }

            context.Recipes.Add(recipe);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Recipes/Edit/5
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            Recipe recipe = context.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            ViewBag.Categories = context.Categories.OrderBy(c => c.Name).ToList();
            return View(recipe);
        }

        // POST: /Recipes/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, Recipe recipe)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = context.Categories.OrderBy(c => c.Name).ToList();
                return View(recipe);
            }

            context.Recipes.Update(recipe);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Recipes/Delete/5
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            Recipe recipe = context.Recipes
                .Include(r => r.Category)
                .FirstOrDefault(r => r.RecipeId == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: /Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            Recipe recipe = context.Recipes.Find(id);
            if (recipe != null)
            {
                context.Recipes.Remove(recipe);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
