using CoreWebApp.Data.Interfaces;
using CoreWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApp.Data.Repositories
{
    public class ProductRepository : RepositoryBase<ProductModel>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateProduct(ProductModel product)
        {
            Create(product);
        }

        public void DeleteProduct(ProductModel product)
        {
            Delete(product);
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await FindAll()
                        .OrderBy(p => p.Title)
                        .ToListAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsByUserId(string userId)
        {
            return await FindByCondition(product => product.UserID.Equals(userId)).ToListAsync();
        }

        public async Task<ProductModel> GetProductByIdAsync(int productId)
        {
            return await FindByCondition(product => product.ID.Equals(productId))
                        .FirstOrDefaultAsync();
        }

        //public async Task<ProductModel> GetProductWithDetailsAsync(Guid productId)
        //{
        //    return await FindByCondition(product => product.Id.Equals(productId))
        //                .Include(ac => ac.ProductsTags)
        //                .ThenInclude(x => x.Tag)
        //                .FirstOrDefaultAsync();
        //}

        public void UpdateProduct(ProductModel product)
        {
            Update(product);
        }
    }
}
