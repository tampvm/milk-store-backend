using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.ProductViewModels
{
    public class UpdateProductDTO
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
        public string UpdatedBy { get; set; }
    }
}
