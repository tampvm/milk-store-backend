using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class EditAccountDTO
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
    }
}
