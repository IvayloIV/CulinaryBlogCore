using System.Collections.Generic;

using CulinaryBlogCore.Models.ChefViewModels;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeHomeViewModel
    {
        public List<RecipeViewModel> RecipesByRating { get; set; }

        public List<RecipeViewModel> LastAddedRecipes { get; set; }

        public List<ChefViewModel> Chefs { get; set; }

        public string CurrUserId { get; set; }
    }
}
