using AutoMapper;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
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
		// Get all active vouchers
		public async Task<ResponseModel> GetVouchersAsync(int pageIndex, int pageSize)
		{
			var vouchers = await _unitOfWork.VoucherRepository.GetAsync(
				filter: x => x.IsDeleted == false,
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

		// Get a voucher by code
		public async Task<ResponseModel> GetVoucherByCodeAsync(string code)
		{
			var voucher = await _unitOfWork.VoucherRepository.GetVoucherByCodeAsync(code);

			if (voucher == null)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Voucher not found."
				};
			}

			var voucherDto = _mapper.Map<ViewDetailVoucherDTO>(voucher);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Voucher retrieved successfully.",
				Data = voucherDto
			};
		}

		// Create a new voucher
		public async Task<ResponseModel> CreateVoucherAsync(CreateVoucherDTO model)
		{
			var voucher = _mapper.Map<Voucher>(model);

			switch (voucher.DiscountType)
			{
				case "FixedAmount" when voucher.DiscountValue <= 0:
					return new ErrorResponseModel<object>
					{
						Success = false,
						Message = "Discount amount must be greater than 0."
					};

				case "Percentage" when voucher.DiscountValue <= 0 || voucher.DiscountValue > 100:
					return new ErrorResponseModel<object>
					{
						Success = false,
						Message = "Discount percentage must be greater than 0 and less than or equal to 100."
					};

				case "FixedAmount":
				case "Percentage":
					break;

				default:
					return new ErrorResponseModel<object>
					{
						Success = false,
						Message = "Invalid discount type."
					};
			}

			voucher.CreatedAt = DateTime.Now;
			await _unitOfWork.VoucherRepository.AddAsync(voucher);
			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Voucher created successfully."
			};
		}

		// Update a voucher
		public async Task<ResponseModel> UpdateVoucherAsync(UpdateVoucherDTO model)
		{
			var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(model.Id);

			if (voucher == null)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Voucher not found."
				};
			}

			switch (model.DiscountType)
			{
				case "FixedAmount" when model.DiscountValue <= 0:
					return new ErrorResponseModel<object>
					{
						Success = false,
						Message = "Discount amount must be greater than 0."
					};

				case "Percentage" when model.DiscountValue <= 0 || model.DiscountValue > 100:
					return new ErrorResponseModel<object>
					{
						Success = false,
						Message = "Discount percentage must be greater than 0 and less than or equal to 100."
					};

				case "FixedAmount":
				case "Percentage":
					break;

				default:
					return new ErrorResponseModel<object>
					{
						Success = false,
						Message = "Invalid discount type."
					};
			}

			_mapper.Map(model, voucher);

			voucher.UpdatedAt = DateTime.Now;
			_unitOfWork.VoucherRepository.Update(voucher);
			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Voucher updated successfully."
			};
		}


		// Delete a voucher
		public async Task<ResponseModel> DeleteVoucherAsync(int id)
		{
			var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(id);

			if (voucher == null)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Voucher not found."
				};
			}

			//voucher.IsDeleted = true;
			voucher.DeletedAt = DateTime.Now;
			_unitOfWork.VoucherRepository.SoftRemove(voucher);
			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Voucher deleted successfully."
			};
		}

		#endregion
	}
}