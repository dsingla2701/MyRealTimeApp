using Core.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyRealTimeApp.Core.Domain.Entities;
using MyRealTimeApp.Infrastructure.Data;

namespace Infrastructure.Repositories;

//this class fulfills the promise made by IProductRepository
//takes abstract commands and writes actual EF code to execute it against SQL server
public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync() => await context.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) => await context.Products.FindAsync(id);

    // <-- NEW METHOD
    public async Task<Product> AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }
}