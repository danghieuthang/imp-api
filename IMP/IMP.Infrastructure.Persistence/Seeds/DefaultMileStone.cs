using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Seeds
{
    public class DefaultMileStone
    {
        public static async Task SeedMilestonesAsync(IUnitOfWork unitOfWork)
        {
            var milestoneRepository = unitOfWork.Repository<Milestone>();
            var mileStones = await milestoneRepository.GetAllAsync();

            if (mileStones.Count == 0)
            {
                var newMilestones = new List<Milestone>
                {
                    new Milestone
                    {
                        NameVi="Nộp Đơn",
                        NameEn="Apply",
                    },
                     new Milestone
                    {
                        NameVi="Duyệt Đơn",
                        NameEn="Approve"
                    },
                      new Milestone
                    {
                        NameVi="Quảng cáo",
                        NameEn="Advertisement"
                    },
                       new Milestone
                    {
                        NameVi="Thông báo",
                        NameEn = "Notify"
                    }
                };

                await milestoneRepository.AddManyAsync(newMilestones);
                await unitOfWork.CommitAsync();

            }

        }
    }
}
