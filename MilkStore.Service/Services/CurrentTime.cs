using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MilkStore.Service.Services.CurrentTime;

namespace MilkStore.Service.Services
{
    public class CurrentTime : ICurrentTime
    {
        public DateTime GetCurrentTime() => DateTime.UtcNow.ToLocalTime();
    }
}
