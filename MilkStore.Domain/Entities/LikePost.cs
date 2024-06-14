using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    public class LikePost : BaseEntity
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key
        public string AccountId { get; set; }
        public int PostId { get; set; }

        // Other Information
        public bool IsLike { get; set; }
        public DateTime LikeAt { get; set; }
        public DateTime UnLikeAt { get; set; }

        // Navigation Property
        public Post Post { get; set; }
        public Account Account { get; set; }
    }
}
