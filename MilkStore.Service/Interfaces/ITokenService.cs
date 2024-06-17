using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string identifier, string method);
        bool ValidateToken(string identifier, string method, string token);
    }
}
