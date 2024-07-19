using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("Product")]
    public class Product : BaseEntity
    {
        // Primary Key
        [Key]
        public string Id { get; set; }

        // Attributes
        public string Sku { get; set; }
		public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public float Weight { get; set; }
        //public bool Status { get; set; }
        public int Quantity { get; set; }

        // Foreign Keys
        [ForeignKey("Type")]
        public int TypeId { get; set; }
		[ForeignKey("AgeRange")]
		public int AgeId { get; set; }
        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        // Other Information
        public bool Active { get; set; }

        // Navigation Properties
        public virtual ProductType Type { get; set; }
        public virtual AgeRange AgeRange { get; set; }
        public virtual Brand Brand { get; set; }
		public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
