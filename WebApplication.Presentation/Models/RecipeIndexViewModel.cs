namespace WebApplication.Presentation.Models
{
    public class RecipeIndexViewModel
    {
        public int CookbookId { get; set; }
        public List<RecipeOverviewViewModel> Recipes { get; set; } = new();
        public RecipeCreateViewModel NewRecipe { get; set; } = new();
    }
}
