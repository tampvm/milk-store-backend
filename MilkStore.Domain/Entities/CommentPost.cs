using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    public class CommentPost : BaseEntity
    {
        // Primary Key
        public int CommentId { get; set; }

        // Attributes
        public string CommentText { get; set; }
        public bool Active { get; set; }

        // Foreign Key
        public int? ParentCommentId { get; set; }
        public string AccountId { get; set; }
        public int PostId { get; set; }

        // Navigation Property
        public Post Post { get; set; }
        public Account Account { get; set; }
        public CommentPost ParentComment { get; set; }
        public ICollection<CommentPost> ChildComments { get; set; } = new List<CommentPost>();
    }
}
