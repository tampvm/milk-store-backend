using AutoMapper;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.VoucherViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
	public class VoucherService : IVoucherService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public VoucherService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		#region Voucher management
		// Get all vouchers
		public async Task<ResponseModel> GetVouchersAsync(int pageIndex, int pageSize)
		{
			var vouchers = await _unitOfWork.VoucherRepository.GetAsync(
				pageIndex: pageIndex,
				pageSize: pageSize
				);

			var voucherDtos = _mapper.Map<Pagination<ViewListVoucherDTO>>(vouchers);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Vouchers retrieved successfully.",
				Data = voucherDtos
			};
		}

		// Create a new voucher
		public async Task<ResponseModel> CreateVoucherAsync(CreateVoucherDTO model)
		{
			var voucher = _mapper.Map<Voucher>(model);

			await _unitOfWork.VoucherRepository.AddAsync(voucher);
			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Voucher created successfully."
			};
		}

		#endregion
	}
}