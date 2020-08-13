using CulinaryBlogCore.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryBlogCore.Models
{
    public class CategoryViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public CategoryViewModel(long id, string name) {
            this.Id = id;
            this.Name = name;
        }
    }
}
