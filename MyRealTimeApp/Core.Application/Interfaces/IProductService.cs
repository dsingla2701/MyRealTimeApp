using MyRealTimeApp.Core.Domain.Entities;

namespace MyRealTimeApp.Core.Application.Interfaces
{

    //contract for business logic operations
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task <Product> AddProductAsync(string name, decimal price);
        Task<Product> UpdateProductPriceAsync(int id, decimal newPrice);

    }
}
