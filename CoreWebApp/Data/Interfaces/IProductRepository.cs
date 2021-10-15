using CoreWebApp.Data.Repositories;
using CoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApp.Data.Interfaces
{
    public interface IProductRepository : IRepositoryBase<ProductModel>
    {
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
        Task<IEnumerable<ProductModel>> GetAllProductsByUserId(string userId);
        Task<ProductModel> GetProductByIdAsync(int productId);
        //Task<ProductModel> GetProductWithDetailsAsync(Guid productId);
        void CreateProduct(ProductModel product);
        void UpdateProduct(ProductModel product);
        void DeleteProduct(ProductModel product);
    }
}
