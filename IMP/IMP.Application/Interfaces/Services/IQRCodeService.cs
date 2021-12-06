using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Services
{
    public interface IQRCodeService
    {
        Task<byte[]> CreateQRCode(string text);
    }
}
