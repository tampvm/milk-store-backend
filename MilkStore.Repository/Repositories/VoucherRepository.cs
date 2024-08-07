﻿using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Repositories
{
	public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
	{
		private readonly AppDbContext _context;

		public VoucherRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
		{
			_context = context;
		}

		public async Task<Voucher?> GetVoucherByCodeAsync(string code)
		{
			return await _context.Vouchers.FirstOrDefaultAsync(x => x.Code.Equals(code));
		}
		public async Task<AccountVoucher> AddAccountVoucher(string AccountId, int VoucherId, string usedDate, string Status)
		{
			var accountVoucher = new AccountVoucher
			{
				AccountId = AccountId,
				VoucherId = VoucherId,
				UsedDate = DateTime.Parse(usedDate),
				Status = Status
			};

			_context.AccountVouchers.Add(accountVoucher);

			await _context.SaveChangesAsync();

			return accountVoucher;
		}
	}
}
