using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class PhoneNumberOrEmailDTO
    {
        public string PhoneNumberOrEmail { get; set; }
    }

    public class VerifyRegisterCodeDTO : PhoneNumberOrEmailDTO
    {
        public string VerificationCode { get; set; }
    }
}
