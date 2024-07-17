using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class ChangePhoneNumberDTO
    {
        public string UserId { get; set; }
        public string NewPhoneNumber { get; set; }
        public string Code { get; set; }
    }

    public class NewPhoneNumberDTO
    {
        public string UserId { get; set; }

        public string NewPhoneNumber { get; set; }
    }
}
