using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.ProductImageViewModels
{
    public class UpdateProductImageDTO
    {
        public int ImageId { get; set; }

        public string ProductId { get; set; }
        public string UpdatedBy { get; set; }
    }
}
