using System.ComponentModel.DataAnnotations;

using CulinaryBlogCore.Services.Models;

namespace CulinaryBlogCore.Models.ChefViewModels
{
    public class UpdateChefViewModel : ImageViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(800, MinimumLength = 20)]
        public string Description { get; set; }
    }
}
