using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
	[Table("PostImage")]
	public class PostImage : BaseEntity
	{
		// Primary key
		[Key] 
		public int Id { get; set; }

		// Foreign keys
		[ForeignKey("Post")]
		public int PostId { get; set; }
		[ForeignKey("Image")]
		public int ImageId { get; set; }

		// Navigation properties
		public virtual Post Post { get; set; }
		public virtual Image Image { get; set; }
	}
}
