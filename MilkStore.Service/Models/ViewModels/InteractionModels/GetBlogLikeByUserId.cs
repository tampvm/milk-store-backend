using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.InteractionModels
{
    public class GetBlogLikeByUserId
    {
        public string AccountId { get; set; }


        public int PostId { get; set; }

        // Other Information
        public bool IsLike { get; set; }
        public DateTime LikeAt { get; set; }
        public DateTime? UnLikeAt { get; set; }
    }
}
