using MilkStore.Domain.Entities;
using MilkStore.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
	public interface IVoucherRepository : IGenericRepository<Voucher>
	{
	}
}
