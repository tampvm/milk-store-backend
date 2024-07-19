using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.InteractionModels
{
    public class UpdateLike
    {    
     
        public bool IsLike { get; set; }
        public DateTime LikeAt { get; set; }
        public DateTime? UnLikeAt { get; set; }
    }
}
