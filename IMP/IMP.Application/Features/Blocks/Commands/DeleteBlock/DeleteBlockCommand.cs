using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System.Linq;
namespace IMP.Application.Features.Blocks.Commands.DeleteBlock
{
    public class DeleteBlockCommand : IDeleteCommand<Block>
    {
        public int Id { get; set; }
        public int InfluencerId { get; set; }
        public class DeleteBlockCommandHandler : DeleteCommandHandler<Block, DeleteBlockCommand>
        {
            public DeleteBlockCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }

            public override async Task<Response<int>> Handle(DeleteBlockCommand request, CancellationToken cancellationToken)
            {

                var entity = await Repository.FindSingleAsync(x => x.Id == request.Id, x => x.Page);
                if (entity == null)
                {
                    var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new ValidationException(error);
                }
                if (entity.Page.InfluencerId != request.InfluencerId)
                {
                    var error = new ValidationError("id", $"Không thể xóa block của người khác nhé cưng.");
                    throw new ValidationException(error);
                }
                Repository.Delete(entity);

                #region Update other block position of page
                var blocks = Repository.GetAll(
                        predicate: x => x.PageId == entity.PageId && x.Position > entity.Position,
                        orderBy: x => x.OrderBy(y => y.Position))
                        .ToList();
                foreach(var block in blocks){
                    block.Position--;
                    Repository.Update(block);
                }
                #endregion

                await UnitOfWork.CommitAsync();
                return new Response<int>(entity.Id);
            }



        }
    }
}