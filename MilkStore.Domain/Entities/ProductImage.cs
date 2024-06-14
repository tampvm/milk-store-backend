using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        // Primary key
        public int Id { get; set; }

        // Foreign keys
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        [ForeignKey("Image")]
        public string ImageId { get; set; }

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Image Image { get; set; }
    }
}
