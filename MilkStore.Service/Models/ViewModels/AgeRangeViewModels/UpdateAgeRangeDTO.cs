using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.AgeRangeViewModels
{
    public class UpdateAgeRangeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string UpdatedBy { get; set; }
        public bool Active { get; set; }
    }
}
