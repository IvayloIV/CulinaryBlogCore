using CulinaryBlogCore.Data.Models.Entities;
using System;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class DeleteViewModel
    {
        public long Id;

        public string Name { get; set; }

        public DateTime? PreparationTime { get; set; }

        public DateTime? CookingTime { get; set; }

        public string ImagePath { get; set; }

        public int? Portions { get; set; }

        public long CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
