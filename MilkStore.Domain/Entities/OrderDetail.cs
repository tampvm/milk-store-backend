using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
	[Table("OrderDetail")]
	public class OrderDetail : BaseEntity
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; } // gia cua tung san pham. De tinh tong thi UnitPrice * Quantity
		[ForeignKey("Order")]
		public string OrderId { get; set; }

		public virtual Product Product { get; set; }
		public virtual Order Order { get; set; }
	}
}
