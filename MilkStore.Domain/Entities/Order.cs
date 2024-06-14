using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
	[Table("Order")]
	public class Order : BaseEntity
	{
		[Key]
		public string Id { get; set; }
		public string ShippingAddress { get; set; }
		public decimal Discount { get; set; }
		public int TotalAmount { get; set; } // Tong tien
		public string Type { get; set; }
		public string Status { get; set; }
		public string PaymentMethod { get; set; }
		public string PaymentStatus { get; set; }
		public int PointUsed { get; set; } // Diem da dung trong don hang nay
		public int PointSaved { get; set; } // Diem kiem duoc trong don hang nay

		// OrderDate = CreatedDate
		[ForeignKey("Customer")]
		public string CustomerId { get; set; }
		[ForeignKey("AccountVoucher")]
		public int AccountVoucherId { get; set; }

		public virtual Account Customer { get; set; }
		public virtual AccountVoucher AccountVoucher { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
		public virtual ICollection<Point> Points { get; set; } = new List<Point>();
	}
}
