using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Domain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

    }

    public interface IBaseEntity
    {
        DateTime CreatedAt { get; set; }

        string? CreatedBy { get; set; }

        DateTime? UpdatedAt { get; set; }

        string? UpdatedBy { get; set; }

        DateTime? DeletedAt { get; set; }

        string? DeletedBy { get; set; }

        bool IsDeleted { get; set; }
    }
}
