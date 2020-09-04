namespace CulinaryBlogCore.Models.ResponseViewModel
{
    public class JsonViewModel
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }

        public JsonViewModel(bool isValid, string message)
        {
            this.IsValid = isValid;
            this.Message = message;
        }
    }
}
