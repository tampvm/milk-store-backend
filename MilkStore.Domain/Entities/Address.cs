using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("Address")]
    public class Address : BaseEntity
    {
        public int Id { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        //public string PhoneNumber { get; set; }

        [ForeignKey("Account")]
        public string AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
