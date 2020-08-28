using System;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class DeleteRecipeViewModel
    {
        public long Id;

        public string Name { get; set; }

        public DateTime? PreparationTime { get; set; }

        public DateTime? CookingTime { get; set; }

        public string ImagePath { get; set; }

        public string ImageId { get; set; }

        public int? Portions { get; set; }

        public long CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
