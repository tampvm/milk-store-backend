using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("Address")]
    public class Address : BaseEntity
    {
        // Primary key
        [Key]
        public int Id { get; set; }

        // Attributes
        public string AddressLine { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        //public string PhoneNumber { get; set; }

        // Foreign Key
        [ForeignKey("Account")]
        public string AccountId { get; set; }

        // Navigation Property
        public virtual Account Account { get; set; }
    }
}
