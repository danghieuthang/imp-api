using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Pages.Commands.DeletePage
{
    public class DeletePageCommand : IDeleteCommand<Page>
    {
        public int InfluencerId { get; set; }
        public int Id { get; set; }
        public class DeletePageCommandHandler : DeleteCommandHandler<Page, DeletePageCommand>
        {
            public DeletePageCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
        }
    }
}