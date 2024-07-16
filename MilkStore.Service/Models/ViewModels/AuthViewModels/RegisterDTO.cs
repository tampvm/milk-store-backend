using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AccountViewModels
{
    public class RegisterDTO
    {
        public string RegisterToken { get; set; } // truyền ẩn trong thẻ input sau khi verify
        public string PhoneNumberOrEmail { get; set; } // truyền ẩn trong thẻ input sau khi verify
        public string FirstName { get; set; } // Nhập
        public string LastName { get; set; } // Nhập
        public string Username { get; set; } // Nhập
        public string Password { get; set; } // Nhập
    }
}
