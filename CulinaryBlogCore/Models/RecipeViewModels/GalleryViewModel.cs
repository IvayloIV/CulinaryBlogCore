using System.Collections.Generic;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class GalleryViewModel
    {
        public string CategoryName { get; set; }

        public List<RecipeViewModel> Recipes { get; set; }

        public List<GalleryViewModel> Galleries { get; set; }

        public GalleryViewModel()
        {
            this.Galleries = new List<GalleryViewModel>();
        }

        public GalleryViewModel(string CategoryName, List<RecipeViewModel> Recipes)
        {
            this.CategoryName = CategoryName;
            this.Recipes = Recipes;
        }
    }
}
