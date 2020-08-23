using CulinaryBlogCore.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CulinaryBlogCore.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<Recipe> Recipes { get; set; }

        public virtual List<UserRecipeRating> UserRecipeRatings { get; set; }
    }
}
