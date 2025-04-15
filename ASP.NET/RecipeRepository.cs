using ASP.NET;

public interface IRecipeRepository
{
    public void Save(RecipeEntity entity);
    public void Remove(RecipeEntity entity);
}

public class EFRecipeRepository : IRecipeRepository
{
    private readonly AppDbContext context;

    public EFRecipeRepository(AppDbContext context)
    {
        this.context = context;
    }

    public void Remove(RecipeEntity entity)
    {
        context.Recipes.Remove(entity);
        context.SaveChanges();
    }

    public void Save(RecipeEntity entity)
    {
        context.Recipes.Add(entity);
        context.SaveChanges();
    }
}