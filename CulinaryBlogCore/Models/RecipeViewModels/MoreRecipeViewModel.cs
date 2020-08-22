using CulinaryBlogCore.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class MoreRecipeViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Time)]
        public DateTime PreparationTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime CookingTime { get; set; }

        public long Rating { get; set; }

        public long VoteCount { get; set; }

        public long ViewCount { get; set; }

        public int Portions { get; set; }

        public string ImagePath { get; set; }

        public List<Product> Products { get; set; }

        public Category Category { get; set; }
    }
}
