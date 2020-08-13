using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;

namespace CulinaryBlogCore.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepository _repository;

        public RecipeService(IRepository repository)
        {
            this._repository = repository;
        }


    }
}
