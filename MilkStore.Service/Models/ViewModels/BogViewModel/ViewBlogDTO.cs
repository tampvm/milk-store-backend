using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.BogViewModel
{
    public class ViewBlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //status
        public bool Status { get; set; }
        public DateTime createAt {  get; set; }
        public string createBy { get; set; } 
        public DateTime updateAt { get; set; }
        public string updateBy { get; set; }
        public DateTime deleteAt { get; set; }
        public string deleteBy { get; set; }
        public bool isDeleted { get; set; }


    }
}
