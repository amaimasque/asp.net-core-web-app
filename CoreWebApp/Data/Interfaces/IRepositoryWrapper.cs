using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApp.Data.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProductRepository Product { get; }

        Task SaveAsync();
    }
}
