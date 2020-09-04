namespace CulinaryBlogCore.Models.ResponseViewModel
{
    public class CreateProductJsonViewModel : JsonViewModel
    {
        public long ProductId { get; set; }

        public CreateProductJsonViewModel(
            bool isValid, 
            string message,
            long productId): base(isValid, message)
        {
            ProductId = productId;
        }
    }
}
