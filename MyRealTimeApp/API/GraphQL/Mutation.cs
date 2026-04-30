using MyRealTimeApp.Core.Application.Interfaces;
using MyRealTimeApp.Core.Domain.Entities;
using HotChocolate;

namespace API.GraphQL

    //changes data (and triggers Application Services)
{

    public class Mutation
    {

        // <-- NEW METHOD
        public async Task<Product> AddProductAsync(string name, decimal price, [Service] IProductService productService)
        {
            return await productService.AddProductAsync(name, price);
        }

        // The [Service] attribute tells HotChocolate to inject your IProductService
        public async Task<Product> UpdateProductPriceAsync(
            int id,
            decimal newPrice,
            [Service] IProductService productService)
        {
            // The service handles all the logic, database saving, and error checking!
            return await productService.UpdateProductPriceAsync(id, newPrice);
        }

    }
}