using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CulinaryBlogCore.Data.Models.Identity;

namespace CulinaryBlogCore.Data.Models.Entities
{
    [Table("Trick")]
    public class Trick
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string ImageId { get; set; }

        public DateTime CreationTime { get; set; }

        public long ChefId { get; set; }

        public Chef Chef { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
