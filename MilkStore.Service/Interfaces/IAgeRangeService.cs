using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AgeRangeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IAgeRangeService
    {
        Task<ResponseModel> GetAllAgeRangeAsync();
        Task<ResponseModel> GetAgeRangeByIdAsync(int id);
        Task<ResponseModel> CreateAgeRangeAsync(CreateAgeRangeDTO createAgeRangeDTO);
        Task<ResponseModel> UpdateAgeRangeAsync(UpdateAgeRangeDTO updateAgeRangeDTO);
        Task<ResponseModel> DeleteAgeRangeAsync(DeleteAgeRangeDTO deleteAgeRangeDTO);
        Task<ResponseModel> RestoreAgeRangeAsync(RestoreAgeRangeDTO restoreAgeRangeDTO);
    }
}
