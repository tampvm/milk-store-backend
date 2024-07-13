﻿using MilkStore.Service.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IBlogService
    {
        Task<ResponseModel> GetAllBlog(int pageIndex, int pageSize);
        Task<ResponseModel> GetBlogByUserId(int pageIndex, int pageSize, int id)
    }
}