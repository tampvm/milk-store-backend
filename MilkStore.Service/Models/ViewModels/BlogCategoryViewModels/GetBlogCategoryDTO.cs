using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.BlogCategoryViewModels
{
    public class GetBlogCategoryDTO
    {
        
        public int Id { get; set; }
        public int PostId { get; set; }
      
        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual Post Post { get; set; }
        public virtual Category Category { get; set; }
    }
}
