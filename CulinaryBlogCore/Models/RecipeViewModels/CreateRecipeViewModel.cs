using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using CulinaryBlogCore.Models.CategoryViewModels;
using CulinaryBlogCore.Services.Models;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class CreateRecipeViewModel : ImageViewModel
    {
        [StringLength(60, MinimumLength = 4)]
        [Required]
        public string Name { get; set; }

        [StringLength(800, MinimumLength = 20)]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Preparation Time")]
        [DataType(DataType.Time)]
        [Required]
        public DateTime? PreparationTime { get; set; }

        [Display(Name = "Cooking Time")]
        [DataType(DataType.Time)]
        [Required]
        public DateTime? CookingTime { get; set; }

        [Range(1, 100)]
        [Required]
        public int? Portions { get; set; }

        [Display(Name = "Category")]
        [Required]
        public long? CategoryId { get; set; }

        public string UserId { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}
