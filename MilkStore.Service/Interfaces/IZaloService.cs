using MilkStore.Service.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MilkStore.Service.Models.ResponseModels.ZaloResponseModel;

namespace MilkStore.Service.Interfaces
{
    public interface IZaloService
    {
        Task<ResponseModel> SendVerificationCodeAsync(string phoneNumber);
    }
}
