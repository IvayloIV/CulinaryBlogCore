using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Models.TrickViewModels
{
    public class TrickViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string UserId { get; set; }

        public string CurrentUserId { get; set; }

        public Chef Chef { get; set; }

        public List<TrickViewModel> Tricks { get; set; }
    }
}
