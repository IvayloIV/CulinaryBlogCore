using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface ITrickService
    {
        void Add(Trick trick);

        List<Trick> GetAll();

        Trick GetById(long id);

        void Delete(Trick trick);

        void Update(long id, Trick newTrick);
    }
}
