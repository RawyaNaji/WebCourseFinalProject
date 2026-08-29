# RecipeBoard

ASP.NET Core MVC (.NET 6) + EF Core final project.

## Setup

1. Install SQL Server Express LocalDB (if not already installed):
   `winget install --id Microsoft.SQLServer.2022.Express -e --silent --accept-package-agreements --accept-source-agreements --override "/ACTION=Install /FEATURES=LocalDB /IACCEPTSQLSERVERLICENSETERMS /Q"`
   (Visual Studio's default "ASP.NET and web development" workload already includes LocalDB, so this step may not be needed on a machine with Visual Studio.)
2. From the `RecipeBoard` folder, restore the local EF Core tool: `dotnet tool restore`
3. Apply the database: `dotnet ef database update`
4. Run the app: `dotnet run` (or open `RecipeBoard.sln` in Visual Studio and press F5)

## Demo accounts

- Admin: `admin / admin123`
- User: `guest / guest123`
- (or sign up your own account via "Sign Up")

## Functionality

- Sign up / Login / Logout / Change Password / View Profile
- Browse and search recipes by title, filter by category
- Save/unsave a recipe to "My Saved Recipes" (per-user, session-driven)
- "Recently viewed" recipe + view counter shown site-wide (session)
- Admin-only: Add / Edit / Delete recipes

## Project structure

- `Models/` — `User`, `Category`, `Recipe`, `SavedRecipe` (junction: User↔Recipe, one row per save)
- `Data/ApplicationDbContext.cs` — EF Core DbContext + seed data
- `Controllers/`:
  - `HomeController`
  - `AccountController` — login/logout/register/change password/profile, all session-driven
  - `RecipesController` — CRUD, search/filter, session "recently viewed"
  - `SavedRecipesController` — save/unsave a recipe, "My Saved Recipes" list (filtered by session UserId)
- `Views/` — Razor views per controller
