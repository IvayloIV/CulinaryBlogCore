using System.Collections.Generic;
using System.Linq;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface ICategoryService
    {
        List<Category> GetAll();

        Category GetById(long id);

        ILookup<string, ICollection<Recipe>> GetRecipesByCategory();
    }
}
