using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("Cart")]
    public class Cart : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Account")]
        public string AccountId { get; set; }

        [ForeignKey("Product")]
        public string ProductId { get; set; }

        public int Quanity { get; set; }
        public string Status { get; set; }

        public virtual Account Account { get; set; }
        public virtual Product Product { get; set; }
    }
}
