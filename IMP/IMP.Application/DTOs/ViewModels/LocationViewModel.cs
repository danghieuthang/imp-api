using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class LocationViewModel : BaseViewModel<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Level { get; set; }
        public LocationViewModel Parent { get; set; }
        public IEnumerable<LocationViewModel> Locations { get; set; }
    }

    public class LocationRequest
    {
        public int LocationId { get; set; }
    }
}
