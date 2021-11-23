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
            await SeedBlockTypeAsync(unitOfWork);
            await SeedRankLevelAsync(unitOfWork);
            await SeedEvidenceTypeAsync(unitOfWork);
            await DefaultLocation.SeedLocationAsync(unitOfWork);
            //await DefaultMileStone.SeedMilestonesAsync(unitOfWork);
        }
        public static async Task SeedBlockTypeAsync(IUnitOfWork unitOfWork)
        {
            var blockTypeRepository = unitOfWork.Repository<BlockType>();
            var blockTypes = await blockTypeRepository.GetAllAsync();
            if (blockTypes.Count == 0)
            {
                var blockTypesDefault = new List<BlockType>{
                    new BlockType{
                        Name="Header Block",
                    },
                    new BlockType{
                        Name="Quote Block",
                    },
                    new BlockType{
                        Name="Link Block",
                    },new BlockType{
                        Name="Youtube Block",
                    }
                    ,new BlockType{
                        Name="Image Block",
                    }
                    ,new BlockType{
                        Name="Product Block",
                    },new BlockType{
                        Name="Voucher Block",
                    },new BlockType{
                        Name="Text Block",
                    },new BlockType{
                        Name="Text Social",
                    }
                };
                await blockTypeRepository.AddManyAsync(blockTypesDefault);
                await unitOfWork.CommitAsync();
            }
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

        public static async Task SeedRankLevelAsync(IUnitOfWork unitOfWork)
        {
            var rankLevelRepository = unitOfWork.Repository<RankLevel>();
            var levels = await rankLevelRepository.GetAllAsync();
            if (levels.Count == 0)
            {
                var rankLevelDefault = new List<RankLevel>
                {
                    new RankLevel
                    {
                        Name="Đồng",
                        Score=0,
                    },
                    new RankLevel
                    {
                        Name="Bạc",
                        Score=100,

                    },
                     new RankLevel
                    {
                        Name="Vàng",
                        Score=1000,

                    },
                     new RankLevel
                    {
                        Name="Kim Cương",
                        Score=100000,
                    },
                };
                await rankLevelRepository.AddManyAsync(rankLevelDefault);
                await unitOfWork.CommitAsync();
            }
        }

        public static async Task SeedEvidenceTypeAsync(IUnitOfWork unitOfWork)
        {
            var evidenceTypeRepository = unitOfWork.Repository<EvidenceType>();
            var evidences = await evidenceTypeRepository.GetAllAsync();
            if (evidences.Count == 0)
            {
                var evidenceTypeDefaul = new List<EvidenceType>
                    {
                        new EvidenceType
                        {
                            Name="Video"
                        },
                        new EvidenceType
                        {
                            Name="Hình ảnh"
                        },
                    };
                await evidenceTypeRepository.AddManyAsync(evidenceTypeDefaul);
                await unitOfWork.CommitAsync();
            }
        }
    }

}