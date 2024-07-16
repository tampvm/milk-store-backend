namespace MilkStore.Service.Models.ViewModels.OrderViewDTO;

public class CreateOrderDTO
{
    public string ShippingAddress { get; set; }
    public decimal Discount { get; set; }
    public int TotalAmount { get; set; } 
    public string Type { get; set; }
    public string Status { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public int? PointUsed { get; set; } 
    public int PointSaved { get; set; } 
    public string AccountId { get; set; }
    
    public int? AccountVoucherId { get; set; }
    public string ProductId { get; set; }
}