using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.BogViewModel
{
    public class CreateBlogDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        public bool Status { get; set; }
        public DateTime createAt { get; set; }
        public string createBy { get; set; }
        public DateTime updateAt { get; set; }
        public string updateBy { get; set; }
        public DateTime deleteAt { get; set; }
        public string deleteBy { get; set; }
        public bool isDeleted { get; set; }
    }
}
