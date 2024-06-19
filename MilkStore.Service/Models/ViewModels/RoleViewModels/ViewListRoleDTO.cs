using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.RoleViewModels
{
    public class ViewListRoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }  // Username
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }  // Username
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; } // Username
        public bool IsDeleted { get; set; }
    }
}
