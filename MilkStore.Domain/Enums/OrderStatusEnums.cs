using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Enums
{
	public enum OrderStatusEnums
	{
		InCart,
		Waiting,
		Preparing,
		Prepared,
		Shipping,
		DeliveryFailed,
		DeliverySuccessful,
		Received,
		Completed
	}
}
