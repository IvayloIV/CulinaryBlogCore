using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IChefService
    {
        void Add(Chef chef);

        List<Chef> GetAll();

        Chef GetById(long id);

        void UpdateById(long id, Chef newChef);

        void DeleteById(long id);
    }
}
