﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.RoleViewModels
{
    public class UpdateRoleDTO
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
    }
}
