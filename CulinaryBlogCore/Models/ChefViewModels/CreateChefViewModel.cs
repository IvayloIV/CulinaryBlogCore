using System.ComponentModel.DataAnnotations;

namespace CulinaryBlogCore.Models.ChefViewModels
{
    public class CreateChefViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 4)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(800, MinimumLength = 20)]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string ImageId { get; set; }
    }
}
