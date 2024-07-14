using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class ViewUserProfileDTO
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? GoogleEmail { get; set; }
        public string? FacebookEmail { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public List<string> Roles { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string? Background { get; set; }
        public int Point { get; set; }
        public List<string> Vouchers { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
