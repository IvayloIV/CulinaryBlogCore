using CulinaryBlogCore.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface ICategoryService
    {
        List<Category> getAll();
    }
}
