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
        // Primary Key
        [Key]
		public int Id { get; set; }

        // Attributes
        public string Status { get; set; }
		public DateTime? UsedDate { get; set; }

        // Foreign Key
        [ForeignKey("Account")]
		public string AccountId { get; set; }

		[ForeignKey("Voucher")]
		public int VoucherId { get; set; }

        // Navigation Property
        public virtual Account Account { get; set; }
		public virtual Voucher Voucher { get; set; }
		public virtual Order Order { get; set; }
}
}
