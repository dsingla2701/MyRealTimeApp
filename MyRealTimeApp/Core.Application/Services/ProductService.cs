using Core.Application.Interfaces;
using MyRealTimeApp.Core.Application.Interfaces;
using MyRealTimeApp.Core.Domain.Entities;

//business logic implementation - here business logic lives
//used primary constructors to inject the repository

//if a product can have -ve price or not
//if email must be sent after the purchase -- all these logic are in ProductService.cs
//The Service uses Repository interfaces to fetch/save data , completely unknown to EF

namespace MyRealTimeApp.Core.Application.Services
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Product> AddProductAsync(string name, decimal price)
        {
            var product = new Product { Name = name, Price = price };
            return await repository.AddAsync(product);
        }

        public async Task<Product> UpdateProductPriceAsync(int id, decimal newPrice)
        {
            var product = await repository.GetByIdAsync(id);

            // Business rule check
            if (product == null)
            {
                throw new System.Collections.Generic.KeyNotFoundException($"Product with ID {id} was not found.");
            }

            if (newPrice < 0)
            {
                throw new ArgumentException("Price cannot be a negative value.");
            }

            product.Price = newPrice;

            // Save the changes via the repository
            await repository.UpdateAsync(product);

            return product;
        }
    }
}
