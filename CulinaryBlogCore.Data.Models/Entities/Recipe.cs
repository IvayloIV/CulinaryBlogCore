﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public long Rating { get; set; }

        public long VoteCount { get; set; }

        public long ViewCount { get; set; }

        public int Portions { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreationTime { get; set; }

        public long CategoryId { get; set; }

        [Required]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
