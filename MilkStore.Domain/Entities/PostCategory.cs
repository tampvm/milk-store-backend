using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    [Table("PostCategory")]
    public class PostCategory : BaseEntity
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Foreign Key
        [ForeignKey("Post")]
        public int PostId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual Post Post { get; set; }
        public virtual Category Category { get; set; }

    }
}
