using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class ChangePhoneNumberDTO : VerifyPhoneNumberDTO
    {
        public string Username { get; set; }
    }

    public class NewPhoneNumberDTO
    {
        public string Username { get; set; }

        public string NewPhoneNumber { get; set; }
    }
}
