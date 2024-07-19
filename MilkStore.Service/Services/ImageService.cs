using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using MilkStore.Service.Models.ViewModels.ImageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
	public class ImageService : IImageService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentTime _currentTime;

		//private readonly UserManager<Account> _userManager;
		public ImageService(IUnitOfWork unitOfWork,
							IMapper mapper,
							ICurrentTime currentTime)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentTime = currentTime;
		}

		// Upload Image
		public async Task<ResponseModel> UploadImageAsync(UploadImageDTO model)
		{
			//var user = await _userManager.FindByIdAsync(model.UserId);
			//if (user == null)
			//{
			//	return new ResponseModel { Success = false, Message = "User not found." };
			//}

			try
			{
				var existingImage = await _unitOfWork.ImageRepository.FindByImageUrlAsync(model.ImageUrl);
				int imgId;
				if (existingImage != null)
				{
					imgId = existingImage.Id;
				}
				else
				{
					// Add new image if it doesn't exist
					var image = _mapper.Map<Image>(model);
					image.CreatedAt = _currentTime.GetCurrentTime();
					//image.CreatedBy = _claimsService.GetCurrentUserId().ToString();
					image.CreatedBy = model.UserId;

					await _unitOfWork.ImageRepository.AddAsync(image);
					await _unitOfWork.SaveChangeAsync();
					imgId = image.Id;
				}

				return new SuccessResponseModel<object>
				{
					Success = true,
					Message = "Image uploaded successfully",
					Data = imgId
				};
			}
			catch (Exception ex)
			{
				return new ErrorResponseModel<string>
				{
					Success = false,
					Message = "An error occurred while uploading the image.",
					Errors = new List<string> { ex.Message }
				};
			}
		}
	}
}
