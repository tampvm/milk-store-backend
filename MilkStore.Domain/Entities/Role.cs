using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MilkStore.Domain.Entities
{
    public class Role : IdentityRole, IBaseEntity
    {
        public Role(string name, string description, bool isDefault, DateTime createdAt, string createdBy, bool isDeleted)
        {
            Name = name;
            Description = description;
            IsDefault = isDefault;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            IsDeleted = isDeleted;
        }

        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
