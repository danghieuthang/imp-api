using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Seeds
{
    public static class DefaultApplication
    {
        public static async Task SeedApplicationData(IUnitOfWork unitOfWork)
        {
            //List<Task> tasks = new();

            //tasks.Add(SeedBankAsync(unitOfWork));
            //tasks.Add(DefaultLocation.SeedLocationAsync(unitOfWork));
            //tasks.Add(DefaultMileStone.SeedMilestonesAsync(unitOfWork));
            //await Task.WhenAll(tasks);

            await SeedBankAsync(unitOfWork);
            await DefaultLocation.SeedLocationAsync(unitOfWork);
            await DefaultMileStone.SeedMilestonesAsync(unitOfWork);

        }
        public static async Task SeedBankAsync(IUnitOfWork unitOfWork)
        {
            var bankRepository = unitOfWork.Repository<Bank>();
            var banks = await bankRepository.GetAllAsync();
            if (banks.Count == 0)
            {
                var locationFile = Path.Combine(Environment.CurrentDirectory, @"App_Datas", "banks.json");
                using (StreamReader sr = File.OpenText(locationFile))
                {
                    var json = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    banks = data.Select(x => new Bank
                    {
                        Code = x.Key,
                        Name = x.Value,
                    }).ToList();
                    await bankRepository.AddManyAsync(banks);
                    await unitOfWork.CommitAsync();
                }
            }
        }
    }
}
