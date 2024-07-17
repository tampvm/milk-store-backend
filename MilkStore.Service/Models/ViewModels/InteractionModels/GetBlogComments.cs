using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.InteractionModels
{
    public class GetBlogComments
    {
        public string AccountId { get; set; }


        public int PostId { get; set; }
        public string CommentText { get; set; }

        // Other Information
      
    }
}
