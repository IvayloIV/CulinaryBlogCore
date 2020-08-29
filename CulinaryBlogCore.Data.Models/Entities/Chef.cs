using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryBlogCore.Data.Models.Entities
{
    [Table("Chef")]
    public class Chef
    {
        [Key]
        public long Id { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string ImageId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
