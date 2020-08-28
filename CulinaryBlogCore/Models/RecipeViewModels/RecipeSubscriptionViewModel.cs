using System;
using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Identity;

namespace CulinaryBlogCore.Models.RecipeViewModels
{
    public class RecipeSubscriptionViewModel
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public DateTime CreationTime { get; set; }

        public virtual ApplicationUser User { get; set; }

        public List<RecipeSubscriptionViewModel> Subscriptions { get; set; }
    }
}
