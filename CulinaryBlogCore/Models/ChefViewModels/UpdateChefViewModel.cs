using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace CulinaryBlogCore.Models.ChefViewModels
{
    public class UpdateChefViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(800, MinimumLength = 20)]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string ImageId { get; set; }

        public IFormFile Image { get; set; }
    }
}
