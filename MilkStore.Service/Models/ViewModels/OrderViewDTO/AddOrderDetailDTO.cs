namespace MilkStore.Service.Models.ViewModels.OrderViewDTO;

public class AddOrderDetailDTO
{
    public string ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; } 
    
    public string OrderId { get; set; }
}