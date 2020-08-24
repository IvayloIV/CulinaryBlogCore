using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeDetailsViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Time)]
        public DateTime PreparationTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime CookingTime { get; set; }

        public string Rating { get; set; }

        public long VoteCount { get; set; }

        public long ViewCount { get; set; }

        public int Portions { get; set; }

        public string ImagePath { get; set; }

        public List<Product> Products { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public string CurrUserId { get; set; }

        public long? UserRating { get; set; }

        public List<RecipeDetailsViewModel> Recipes { get; set; }
    }
}
