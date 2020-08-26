using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CulinaryBlogCore.Data.Models.Identity;

namespace CulinaryBlogCore.Data.Models.Entities
{
    [Table("RecipeSubscription")]
    public class RecipeSubscription
    {
        [Key]
        public long Id { get; set; }

        public string Email { get; set; }

        public DateTime CreationTime { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
