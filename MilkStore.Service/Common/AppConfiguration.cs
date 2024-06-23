using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Common
{
    public class AppConfiguration
    {
        public string DatabaseConnection { get; set; }
        public JWTSettings JWT { get; set; }
        public TwilioSettings Twilio { get; set; }
        public ZaloSettings Zalo { get; set; }
        public EmailSettings Email { get; set; }
        public FirebaseSettings Firebase { get; set; }
        public AuthenticationSettings Authentication { get; set; }
    }
}
