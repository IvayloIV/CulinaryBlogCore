using System;

using Microsoft.AspNetCore.Http;

namespace CulinaryBlogCore.Models.TrickViewModels
{
    public class CreateTrickViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string ImageId { get; set; }

        public IFormFile Image { get; set; }

        public long ChefId { get; set; }
    }
}
