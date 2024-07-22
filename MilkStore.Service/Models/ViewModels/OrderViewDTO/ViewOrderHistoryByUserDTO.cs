using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.OrderViewDTO
{
    public class ViewOrderHistoryByUserDTO
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }

    public class ViewOrderHistoryDetailDTO
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public CustomerDTO Customer { get; set; }
        public RecipientDTO Recipient { get; set; }
        public PaymentDTO Payment { get; set; }
        public List<ProductDTO> Products { get; set; }


    }
}
