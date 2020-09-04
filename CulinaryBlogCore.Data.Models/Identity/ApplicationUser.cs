using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;

using Microsoft.AspNetCore.Identity;

namespace CulinaryBlogCore.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<Recipe> Recipes { get; set; }

        public virtual List<UserRecipeRating> UserRecipeRatings { get; set; }

        public virtual List<Trick> Tricks { get; set; }
    }
}
