namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeRatingViewModel
    {
        public virtual string UserId { get; set; }

        public virtual long RecipeId { get; set; }

        public int Rating { get; set; }
    }
}
