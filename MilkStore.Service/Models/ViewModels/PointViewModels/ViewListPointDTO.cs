using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.PointViewModels
{
	public class ViewListPointDTO
	{
		public int Id { get; set; }
		public int Points { get; set; }
		public string TransactionType { get; set; } // earning or spending

		// TransactionDate = CreatedDate: Ngay giao dich

		public string AccountId { get; set; }
		public string OrderId { get; set; }
	}
}
