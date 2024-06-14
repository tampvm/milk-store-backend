using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("Feedback")]
    public class Feedback : BaseEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int Rate { get; set; }

        [ForeignKey("Account")]
        public string AccountId { get; set; }

        [ForeignKey("Product")]
        public string ProductId { get; set; }

        public virtual Account Account { get; set; }

        public virtual Product Product { get; set; }
    }
}
