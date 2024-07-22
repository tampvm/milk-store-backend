using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AgeRangeViewModels
{
    public class CreateAgeRangeDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public bool Active { get; set; }
    }
}
