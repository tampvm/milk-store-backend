using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    public class PostImage : BaseEntity
    {
        // Primary key
        public int Id { get; set; }

        // Foreign keys
        [ForeignKey("Post")]
        public string PostId { get; set; }
        [ForeignKey("Image")]
        public string ImageId { get; set; }

        // Navigation properties
        public virtual Post Post { get; set; }
        public virtual Image Image { get; set; }
    }
}
