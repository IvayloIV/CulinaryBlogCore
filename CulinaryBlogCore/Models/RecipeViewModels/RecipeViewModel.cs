namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public double Rating { get; set; }
    }
}
