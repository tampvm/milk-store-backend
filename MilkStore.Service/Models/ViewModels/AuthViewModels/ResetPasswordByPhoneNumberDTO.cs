using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class ResetPasswordByPhoneNumberDTO
    {
        public string PhoneNumber { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }

}
