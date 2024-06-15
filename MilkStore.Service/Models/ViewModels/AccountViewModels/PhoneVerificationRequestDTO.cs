using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class RegisterPhoneNumberDTO
    {
        public string PhoneNumber { get; set; }
    }

    public class VerifyPhoneNumberDTO : RegisterPhoneNumberDTO
    {
        public string Code { get; set; }
    }

    public class ChangePhoneNumberDTO : RegisterPhoneNumberDTO
    {
        public string Username { get; set; }
    }

    public class PhoneVerificationRequestDTO : ChangePhoneNumberDTO
    {
        public string Code { get; set; }
    }
}
