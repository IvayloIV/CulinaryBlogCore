namespace CulinaryBlogCore.Models.ProductViewModels
{
    public class CreateProductViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long RecipeId { get; set; }
    }
}
