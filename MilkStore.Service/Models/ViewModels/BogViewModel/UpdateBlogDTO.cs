using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.BogViewModel
{
    public class UpdateBlogDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public DateTime UpdateAt { get; set; }
        public string UpdateBy { get; set; }

    }
}
