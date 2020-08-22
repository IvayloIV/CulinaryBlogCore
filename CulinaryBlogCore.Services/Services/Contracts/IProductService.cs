using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IProductService
    {
        void Add(Product product);

        void RemoveById(long id);

        Product GetById(long id);

        bool CheckIfExist(long recipeId, string productName);
    }
}
