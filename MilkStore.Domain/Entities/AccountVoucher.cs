using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
	[Table("AccountVoucher")]
	public class AccountVoucher : BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public string Status { get; set; }
		public DateTime? UsedDate { get; set; }
		[ForeignKey("Account")]
		public string AccountId { get; set; }
		[ForeignKey("Voucher")]
		public int VoucherId { get; set; }
		public virtual Account Account { get; set; }
		public virtual Voucher Voucher { get; set; }
		public virtual Order Order { get; set; }
}
}
