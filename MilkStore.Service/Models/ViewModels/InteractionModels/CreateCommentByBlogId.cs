﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.InteractionModels
{
    public class CreateCommentByBlogId
    {
        public string CommentText { get; set; }
        public bool Active { get; set; }    
    }
}
