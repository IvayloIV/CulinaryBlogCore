using System.Collections.Generic;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeHomeViewModel
    {
        public List<RecipeViewModel> recipesByRating { get; set; }

        public List<RecipeViewModel> lastAddedRecipes { get; set; }

        public string CurrUserId { get; set; }
    }
}
