using CulinaryBlogCore.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public double Rating { get; set; }

        public int Portions { get; set; }

        public string ImagePath { get; set; }

        public List<Product> Products { get; set; }

        public List<RecipeDetailsViewModel> Recipes { get; set; }
    }
}
