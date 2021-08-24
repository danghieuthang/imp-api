using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<int, Product>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
    }
}
