using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("CommentPost")]
    public class CommentPost : BaseEntity
    {
        // Primary Key
        [Key]
        public int CommentId { get; set; }

        // Attributes
        public string CommentText { get; set; }
        public bool Active { get; set; }

        // Foreign Key
        [ForeignKey("ParentComment")]
        public int? ParentCommentId { get; set; }
        [ForeignKey("Account")]
        public string AccountId { get; set; }
		[ForeignKey("Post")]
		public int PostId { get; set; }

        // Navigation Property
        public virtual Post Post { get; set; }
        public virtual Account Account { get; set; }
        public virtual CommentPost ParentComment { get; set; }
        public virtual ICollection<CommentPost> ChildComments { get; set; } = new List<CommentPost>();
    }
}
