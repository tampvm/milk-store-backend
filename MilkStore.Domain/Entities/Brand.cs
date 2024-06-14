using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("Brand")]
    public class Brand : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string BrandOrigin { get; set; }

        public string? Description { get; set; }

        public bool Active { get; set; }

        [ForeignKey("Image")]
        public int ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<FollowBrand> FollowBrands { get; set; } = new List<FollowBrand>();

    }
}
