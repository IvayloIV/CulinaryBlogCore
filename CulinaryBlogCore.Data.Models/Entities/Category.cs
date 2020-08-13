using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryBlogCore.Data.Models.Entities
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
