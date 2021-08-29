using IMP.Application.DTOs.ViewModels;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.Platforms.Commands.UpdatePlatform
{
    public class UpdatePlatformCommand: IRequest<Response<PlatformViewModel>>
    {

    }
}
