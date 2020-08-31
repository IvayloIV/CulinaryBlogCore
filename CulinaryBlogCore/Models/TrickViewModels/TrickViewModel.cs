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

        public Chef Chef { get; set; }

        public List<TrickViewModel> Tricks { get; set; }
    }
}
