using ASP.NET;

public class RecipeService
{
    private IRecipeRepository recipeRepository;

    public RecipeService(IRecipeRepository recipeRepository)
    {
        this.recipeRepository = recipeRepository;
    }

    public RecipeEntity CreateRecipe()
    {
        var recipe = new RecipeEntity("Pannkakor", 10.0, "");

        recipeRepository.Save(recipe);

        return recipe;
    }
}