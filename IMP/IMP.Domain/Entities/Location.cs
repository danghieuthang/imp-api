using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    [Table("Locations")]
    public class Location : BaseEntity
    {
        public Location()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
            Locations = new HashSet<Location>();
        }
        [StringLength(256)]
        public string Code { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Slug { get; set; }
        [StringLength(256)]
        public string Level { get; set; }
        public int? ParentId { get; set; }
        public Location Parent { get; set; }


        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<TargetConfigurationLocation> TargetCustomerConfigurations {get; set; }
        public ICollection<InfluencerConfigurationLocation> InfluencerConfigurationLocations { get; set; }
    }
}
