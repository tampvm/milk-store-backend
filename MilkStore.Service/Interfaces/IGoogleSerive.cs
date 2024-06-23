using MilkStore.Service.Models.ViewModels.AuthViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IGoogleSerive
    {
        Task<GoogleTokenResponse> ExchangeAuthCodeForTokensAsync(string authCode);
        Task<GooglePayload> VerifyGoogleTokenAsync(string token);
    }
}
