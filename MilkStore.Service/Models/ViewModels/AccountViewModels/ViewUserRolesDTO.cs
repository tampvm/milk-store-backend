using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class ViewUserRolesDTO
    {
        public string UserId { get; set; }
        public List<string> RoleNames { get; set; }   
    }

    public class ViewNotAssignedUserRolesDTO
    {
        public List<string> RoleNames { get; set; }
    }
}
