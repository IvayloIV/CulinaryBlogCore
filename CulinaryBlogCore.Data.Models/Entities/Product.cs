using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryBlogCore.Data.Models.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public long RecipeId { get; set; }

        [Required]
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }
    }
}
