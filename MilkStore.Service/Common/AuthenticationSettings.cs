using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Common
{
    public class AuthenticationSettings
    {
        public GoogleSettings Google { get; set; }
        public FacebookSettings Facebook { get; set; }
    }
}
