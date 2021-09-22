using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Seeds
{
    public class LocationObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name_with_type")]
        public string NameWithType { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public static class DefaultLocation
    {
        public static string LocationFile = "vietnam_cities.json";
        public static async Task SeedAsync(IUnitOfWork unitOfWork)
        {
            var locationRepository = unitOfWork.Repository<Location>();
            var locations = await locationRepository.GetAllAsync();

            if (locations.Count == 0)
            {
                var locationFile = Path.Combine(Environment.CurrentDirectory, @"App_Datas", LocationFile);
                using (StreamReader rd = File.OpenText(locationFile))
                {
                    var json = rd.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<Dictionary<int, LocationObject>>(json);
                    locations = data.Select(x => new Location
                    {
                        Name = x.Value.Name,
                        Code = x.Value.Code,
                        Slug = x.Value.Slug,
                        Level = "tỉnh",
                    }).ToList();
                    await locationRepository.AddManyAsync(locations);
                    await unitOfWork.CommitAsync();
                }
            }

        }
    }
}
