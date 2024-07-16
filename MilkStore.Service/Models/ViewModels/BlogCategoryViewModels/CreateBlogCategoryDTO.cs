using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.BlogCategoryViewModels
{
    public class CreateBlogCategoryDTO
    {
        [Required(ErrorMessage = "PostId is required.")]
        public int PostId { get; set; }
        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }
       
    }
}
