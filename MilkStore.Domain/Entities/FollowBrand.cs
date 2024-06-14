using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("FollowBrand")]
    public class FollowBrand : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool IsFollow { get; set; }
        public DateTime FollowedAt { get; set; }
        public DateTime? UnfollowedAt { get; set; }

        [ForeignKey("Account")]
        public string AccountId { get; set; }
        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Brand Brand { get; set; }
    }
}
