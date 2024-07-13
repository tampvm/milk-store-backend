using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.VoucherViewModels
{
	public class UpdateVoucherDTO
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public string DiscountType { get; set; } // % or so tien
		public decimal DiscountValue { get; set; } // so % or so tien giam gia
		public DateTime? StartDate { get; set; } // ngay bat dau ap dung giam gia
		public DateTime? EndDate { get; set; } // ngay ket thuc ap dung giam gia
		public int? UsageLimit { get; set; } // gioi han bao nhieu nguoi su dung
		public int? UsedCount { get; set; } // neu duoc su dung thi UsedCount dem len, UsedCount = UsageLimit thi khong duoc dung nua
		public decimal MiniumOrderValue { get; set; } // don hang bao nhieu tien thi duoc ap dung
		public string Status { get; set; }
	}
}
