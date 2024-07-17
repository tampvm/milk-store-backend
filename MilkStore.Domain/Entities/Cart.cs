using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkStore.Domain.Entities;
[Table("Cart")]
public class Cart : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string AccountId { get; set; }
    public string ProductId { get; set; }
    public string Status { get; set; }
    [ForeignKey("AccountId")]
    public virtual Account Account { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
}