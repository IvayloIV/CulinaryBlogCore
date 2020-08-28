using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryBlogCore.Data.Models.Entities
{
    [Table("ImgurToken")]
    public class ImgurToken
    {
        [Key]
        public long Id { get; set; }

        public string Token { get; set; }

        public int ExpiresIn { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
