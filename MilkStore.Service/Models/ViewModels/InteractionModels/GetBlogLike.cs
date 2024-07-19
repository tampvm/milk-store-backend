using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.InteractionModels
{
    public class GetBlogLike
    {
            // Foreign Key
           
            public string AccountId { get; set; }

          
            public int PostId { get; set; }

            // Other Information
            public bool IsLike { get; set; }
            public DateTime LikeAt { get; set; }
            public DateTime? UnLikeAt { get; set; }

           
     }
    
}
