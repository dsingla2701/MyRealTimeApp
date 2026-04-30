using HotChocolate;
using MyRealTimeApp.Core.Application.Interfaces;
using MyRealTimeApp.Core.Domain.Entities;

namespace API.GraphQL;

//fetch data
public class Query
{
    // This satisfies the GraphQL requirement and gives you a way to read data via GraphQL if you want to!
    public async Task<IEnumerable<Product>> GetProductsAsync([Service] IProductService productService)
    {
        return await productService.GetAllProductsAsync();
    }
}