using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("Image")]
    public class Image : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Type { get; set; }

        // Navigation properties
        public virtual Brand Brand { get; set; }
        public virtual ICollection<Account> AccountAvatars { get; set; } = new List<Account>();
        public virtual ICollection<Account> AccountBackgrounds { get; set; } = new List<Account>();
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public virtual ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();
    }
}
