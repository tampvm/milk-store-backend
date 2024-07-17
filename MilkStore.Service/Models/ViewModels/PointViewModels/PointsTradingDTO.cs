using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.PointViewModels
{
	public class PointsTradingDTO
	{
		public int Points { get; set; }
		public string TransactionType { get; set; } // earning or spending
		public string AccountId { get; set; }
		public string OrderId { get; set; }
	}
}
