using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class PhoneNumberDTO
    {
        public string PhoneNumber { get; set; }
    }

    public class VerifyPhoneNumberDTO : PhoneNumberDTO
    {
        public string Code { get; set; }
    }
}
