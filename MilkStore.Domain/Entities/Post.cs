using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("Post")]
    public class Post : BaseEntity
    {
        // Primary Key
        [Key]
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
