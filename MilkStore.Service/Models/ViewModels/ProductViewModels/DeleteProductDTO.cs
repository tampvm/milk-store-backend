﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.ProductViewModels
{
    public class DeleteProductDTO
    {
        public string Id { get; set; }

        public string DeletedBy { get; set; }
    }
}
