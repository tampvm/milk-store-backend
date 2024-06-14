using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    public class Product : BaseEntity
    {
        // Primary Key
        public int Id { get; set; }

        // Attributes
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public float Weight { get; set; }
        public bool Status { get; set; }
        public int Quantity { get; set; }

        // Foreign Keys
        public int TypeId { get; set; }
        public int AgeId { get; set; }
        public int BrandId { get; set; }
        public int ImageId { get; set; }

        // Other Information
        public bool Active { get; set; }

        // Navigation Properties
        public virtual Type Type { get; set; }
        public virtual AgeRange AgeRange { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
