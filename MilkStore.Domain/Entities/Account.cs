using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    public class Account : IdentityUser, IBaseEntity
    {
        // Attributes
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? GoogleEmail { get; set; }
        public string? FacebookEmail { get; set; }
        public string Gender { get; set; } // false la nam, true la nu
        public string Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int TotalPoints { get; set; }

        // Other Information
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } // duoc tao mac dinh false

        // Foreign Keys
        public int? AvatarId { get; set; }
		public int? BackgroundId { get; set; }

        // Navigation Properties
        public virtual Image Avatar { get; set; }
        public virtual Image Background { get; set; }
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();
        public virtual ICollection<LikePost> LikePosts { get; set; } = new List<LikePost>();
        public virtual ICollection<FollowBrand> FollowBrands { get; set; } = new List<FollowBrand>();
        public virtual ICollection<CommentBrand> CommentBrands { get; set; } = new List<CommentBrand>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Point> Points { get; set; } = new List<Point>();
        public virtual ICollection<AccountVoucher> AccountVouchers { get; set; } = new List<AccountVoucher>();
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        //public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
