using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    public class Post : BaseEntity
    {
        // Primary Key
        public int Id { get; set; }

        // Attributes
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }

        // Foreign Key
        //public string AuthorId { get; set; }  = createBy

        // Navigation Properties
        //public virtual Account Author { get; set; }
        public virtual ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
        public virtual ICollection<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();
        public virtual ICollection<LikePost> LikePosts { get; set; } = new List<LikePost>();
        public virtual ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();
    }
}
