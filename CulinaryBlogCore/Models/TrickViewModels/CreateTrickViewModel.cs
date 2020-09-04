using CulinaryBlogCore.Services.Models;

namespace CulinaryBlogCore.Models.TrickViewModels
{
    public class CreateTrickViewModel : ImageViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long ChefId { get; set; }
    }
}
