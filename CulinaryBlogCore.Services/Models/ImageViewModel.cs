using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace CulinaryBlogCore.Services.Models
{
    public class ImageViewModel
    {
        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string ImageId { get; set; }

        public IFormFile Image { get; set; }
    }
}
