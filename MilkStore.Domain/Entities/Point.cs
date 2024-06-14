using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
	[Table("Point")]
	public class Point : BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public int Points { get; set; }
		public string TransactionType { get; set; } // earning or spending

		// TransactionDate = CreatedDate: Ngay giao dich
		[ForeignKey("Account")]
		public string AccountId { get; set; }
		[ForeignKey("Order")]
		public string OrderId { get; set; }

		public virtual Account Account { get; set; }
		public virtual Order Order { get; set; }
	}
}
