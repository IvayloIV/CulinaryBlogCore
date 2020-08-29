using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string Rating { get; set; }

        public long VoteCount { get; set; }

        public string UserId { get; set; }

        public long? UserRating { get; set; }

        public Category Category { get; set; }
    }
}
