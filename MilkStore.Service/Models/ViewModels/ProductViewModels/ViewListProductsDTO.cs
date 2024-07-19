using MilkStore.Domain.Entities;
using MilkStore.Service.Models.ViewModels.AgeRangeViewModels;
using MilkStore.Service.Models.ViewModels.BrandViewModels;
using MilkStore.Service.Models.ViewModels.ProductTypeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.ProductViewModels
{
    public class ViewListProductsDTO
    {
        public string Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public float Weight { get; set; }
        public int Quantity { get; set; }
        public int TypeId { get; set; }
        public int AgeId { get; set; }
        public int BrandId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public ViewBrandDetailDTO Brand { get; set; }
        public ViewListProductTypeDTO Type { get; set; }
        public ViewListAgeRangeDTO AgeRange { get; set; }
    }
}
