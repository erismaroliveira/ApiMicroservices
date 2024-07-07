using ApiMicroservicesProduct.Models;
using ApiMicroservicesProduct.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiMicroservicesProduct.Context.Repositories;

public class ProductRepository(AppDbContext appDbContext) : IProductRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<IEnumerable<Product>> GetItemsAsync()
    {
        return await _appDbContext.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoriesAsync(string categoryStr)
    {
        return await _appDbContext.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.Category.Name.Equals(categoryStr))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetSearchProductsAsync(string keyword)
    {
        var products = await _appDbContext.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .ToListAsync();

        var filteredProducts = products
            .Where(x => x.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) || 
                x.Category.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        return filteredProducts;
    }

    public async Task<Product> GetByIdAsync(int? id)
    {
        return await _appDbContext.Products
            .Include(x => x.Category)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Product> CreateAsync(Product entity)
    {
        _appDbContext.Products.Add(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Product> UpdateAsync(Product entity)
    {
        _appDbContext.Products.Update(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Product> DeleteAsync(Product entity)
    {
        _appDbContext.Products.Remove(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }
}
