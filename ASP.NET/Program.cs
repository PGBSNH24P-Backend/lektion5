using Microsoft.EntityFrameworkCore;

namespace ASP.NET;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql("Host=localhost;Database=lektion5;Username=postgres;Password=password"));

        var app = builder.Build();

        TestEF(app);

        app.Run();
    }

    static void TestEF(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // CREATE
        // Git-commit-aktig: säg att vi vill spara, men den sparar inte än
        dbContext.Recipes.Add(new RecipeEntity("Tacos", 10.0, "1. Stek köttfärs"));
        // Git-push-aktik: pusha upp ändringarna (.Add från innan)
        dbContext.SaveChanges();

        // READ
        var recipes = dbContext.Recipes.Where(recipe => recipe.Title.Equals("Pannkakor")).ToList();
        foreach (var recipe in recipes)
        {
            Console.WriteLine("Recipe title: " + recipe.Title);

            // UPDATE
            recipe.Rating = 8.0;
            dbContext.SaveChanges();
        }

        // READ
        var tacosRecipe = dbContext.Recipes
            .Where(recipe => recipe.Title.Equals("Tacos"))
            .FirstOrDefault();
        if (tacosRecipe == null)
        {
            Console.WriteLine("Tacos hittas inte!");
        }
        else
        {
            // DELETE
            dbContext.Recipes.Remove(tacosRecipe);
            dbContext.SaveChanges();
            Console.WriteLine("Vi hittade Tacos! Nu är den raderad!");
        }


    }
}

public class RecipeEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Instructions { get; set; } = string.Empty;

    // Constructor för vårt eget bruk
    public RecipeEntity(string title, double rating, string instructions)
    {
        this.Id = Guid.NewGuid();
        this.Title = title;
        this.Rating = rating;
        this.Instructions = instructions;
    }

    // Entity Framework kräver att vi har en tom constructor
    public RecipeEntity() { }
}

public class AppDbContext : DbContext
{
    public DbSet<RecipeEntity> Recipes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}