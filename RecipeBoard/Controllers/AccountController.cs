using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RecipeBoard.Data;
using RecipeBoard.Models;

namespace RecipeBoard.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;

        public AccountController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            User user = context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "Recipes");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Username and password are required.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View();
            }

            bool usernameTaken = context.Users.Any(u => u.Username == username);
            if (usernameTaken)
            {
                ViewBag.ErrorMessage = "That username is already taken.";
                return View();
            }

            User newUser = new User
            {
                Username = username,
                Password = password,
                Role = "User"
            };

            context.Users.Add(newUser);
            context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("Username", newUser.Username);
            HttpContext.Session.SetString("Role", newUser.Role);

            return RedirectToAction("Index", "Recipes");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            User user = context.Users.Find(userId.Value);

            if (user.Password != currentPassword)
            {
                ViewBag.ErrorMessage = "Current password is incorrect.";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "New passwords do not match.";
                return View();
            }

            user.Password = newPassword;
            context.SaveChanges();

            ViewBag.SuccessMessage = "Password changed successfully.";
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            User user = context.Users.Find(userId.Value);
            int savedCount = context.SavedRecipes.Count(s => s.UserId == userId.Value);
            ViewBag.SavedCount = savedCount;

            return View(user);
        }
    }
}
