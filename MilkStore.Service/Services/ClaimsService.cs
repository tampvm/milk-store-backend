using Microsoft.AspNetCore.Http;
using MilkStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("userId");
            return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        }

        //public ClaimsService(IHttpContextAccessor httpContextAccessor)
        //{
        //    // todo implementation to get the current userId
        //    var Id = httpContextAccessor.HttpContext?.User?.FindFirstValue("userId");
        //    GetCurrentUserId = string.IsNullOrEmpty(Id) ? Guid.Empty : Guid.Parse(Id);
        //}

        //public Guid GetCurrentUserId { get; }

    }
}
