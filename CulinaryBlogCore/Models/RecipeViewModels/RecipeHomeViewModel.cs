using System.Collections.Generic;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeHomeViewModel
    {
        public List<RecipeViewModel> RecipesByRating { get; set; }

        public List<RecipeViewModel> LastAddedRecipes { get; set; }

        public string CurrUserId { get; set; }
    }
}
