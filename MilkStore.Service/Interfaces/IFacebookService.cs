using MilkStore.Service.Models.ViewModels.AuthViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IFacebookService
    {
        Task<FacebookUserInfo> GetUserInfoFromFacebookAsync(string accessToken);
    }
}
