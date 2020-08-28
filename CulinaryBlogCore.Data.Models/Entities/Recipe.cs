using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CulinaryBlogCore.Data.Models.Identity;

namespace CulinaryBlogCore.Data.Models.Entities
{
    [Table("Recipe")]
    public class Recipe
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime PreparationTime { get; set; }

        public DateTime CookingTime { get; set; }

        public long ViewCount { get; set; }

        public int Portions { get; set; }

        public string ImageId { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreationTime { get; set; }

        public long CategoryId { get; set; }

        [Required]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<UserRecipeRating> UserRecipeRatings { get; set; }
    }
}
