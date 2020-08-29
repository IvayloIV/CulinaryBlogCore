using System.Collections.Generic;

using CulinaryBlogCore.Models.CategoryViewModels;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class GalleryViewModel
    {
        public List<RecipeViewModel> Recipes { get; set; }

        public List<CategoryViewModel> Categories { get ;set; }
    }
}
