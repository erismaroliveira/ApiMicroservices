using ApiMicroservicesProduct.Models;
using ApiMicroservicesProduct.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiMicroservicesProduct.Context.Repositories;

public class CategoryRepository(AppDbContext appDbContext) : ICategoryRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<IEnumerable<Category>> GetItemsAsync()
    {
        return await _appDbContext.Categories
            .AsNoTracking()
            .Include(x => x.Products)
            .ToListAsync();
    }

    public async Task<Category> GetByIdAsync(int? id)
    {
        return await _appDbContext.Categories
            .Include(x => x.Products)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Category> CreateAsync(Category entity)
    {
        _appDbContext.Categories.Add(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Category> UpdateAsync(Category entity)
    {
        _appDbContext.Categories.Update(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Category> DeleteAsync(Category entity)
    {
        _appDbContext.Categories.Remove(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }
}
