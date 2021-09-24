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
    public static class DefaultBank
    {
        public static async Task SeedAsync(IUnitOfWork unitOfWork)
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

            // Seed transaction type
            //var transactionTypeRepository = unitOfWork.Repository<TransactionType>();
            //var transactionTypes = await transactionTypeRepository.GetAllAsync();
            //if (transactionTypes.Count == 0)
            //{
            //    List<TransactionType> transactionTypeDomain = new List<TransactionType>
            //    {
            //        new TransactionType
            //        {
            //             Id=1,
            //             Name="Recharge",
            //             Description = "Nạp tiền"
            //        },
            //        new TransactionType
            //        {
            //             Id=2,
            //             Name="Withdrawal",
            //             Description = "Rút tiền"
            //        }
            //    };
            //    await transactionTypeRepository.AddManyAsync(transactionTypeDomain);
            //    await unitOfWork.CommitAsync();
            //}
        }
    }
}
