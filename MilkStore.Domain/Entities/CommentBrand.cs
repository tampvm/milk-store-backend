using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
	[Table("CommentBrand")]
	public class CommentBrand : BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public string CommentText { get; set; }
		public bool Active { get; set; }
		
		[ForeignKey("Account")]
		public string AccountId { get; set; }
		[ForeignKey("Brand")]
		public int BrandId { get; set; }
		[ForeignKey("ParentComment")]
		public int? ParentCommentId { get; set; }

		public virtual Account Account { get; set; }
		public virtual Brand Brand { get; set; }
		public virtual CommentBrand ParentComment { get; set; }
		public virtual ICollection<CommentBrand> ChildComments { get; set; } = new List<CommentBrand>();
	}
}
