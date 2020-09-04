namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeRatingViewModel
    {
        public string UserId { get; set; }

        public long RecipeId { get; set; }

        public int Rating { get; set; }
    }
}
