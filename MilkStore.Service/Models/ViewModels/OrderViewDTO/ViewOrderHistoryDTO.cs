using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.OrderViewDTO
{
    public class ViewOrderHistoryDTO
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerDTO Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public RecipientDTO Recipient { get; set; }
        public PaymentDTO Payment { get; set; }
        public List<ProductDTO> Products { get; set; }
    }

    public class CustomerDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class RecipientDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class PaymentDTO
    {
        public string Cash { get; set; }
        public string VnpayQR { get; set; }
        public string Momo { get; set; }
        public string Paypal { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public string ShippingFee { get; set; }
        public string Coupon { get; set; }
        public string Points { get; set; }
        public decimal Total { get; set; }
    }

    public class ProductDTO
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
