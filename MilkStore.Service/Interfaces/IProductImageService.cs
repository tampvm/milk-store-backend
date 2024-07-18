using Microsoft.AspNetCore.Http;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.ProductImageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IProductImageService
    {
        Task<ResponseModel> GetProductImagesAsync(string productId);
        Task<ResponseModel> CreateProductImageAsync(CreateProductImageDTO createProductImageDTO, List<IFormFile> imageFiles, IFormFile thumbnailFile);
        Task<ResponseModel> UpdateProductImageAsync(UpdateProductImageDTO updateProductImageDTO, IFormFile imageFile, IFormFile thumbnailFile);
        Task<ResponseModel> DeleteProductImageAsync(DeleteProductImageDTO deleteProductImageDTO);
    }
}
