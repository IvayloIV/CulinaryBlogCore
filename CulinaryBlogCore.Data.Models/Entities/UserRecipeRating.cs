using CulinaryBlogCore.Data.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryBlogCore.Data.Models.Entities
{
    [Table("UserRecipeRating")]
    public class UserRecipeRating
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public long RecipeId  { get; set; }

        public virtual Recipe Recipe { get; set; }

        public int Rating { get; set; }
    }
}
