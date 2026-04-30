using MyRealTimeApp.Core.Domain.Entities;

namespace Core.Application.Interfaces;

//contract for how data is accessed -> no context if it is SQL server or txt file
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product); // <-- NEW
    Task UpdateAsync(Product product);
}