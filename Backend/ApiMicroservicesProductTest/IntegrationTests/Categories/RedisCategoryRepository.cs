using ApiMicroservicesProduct.Models;
using ApiMicroservicesProduct.Models.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ApiMicroservicesProductTest.IntegrationTests.Categories;

public class RedisCategoryRepository(IDatabase cache) : ICategoryRepository
{
    private readonly IDatabase _cache = cache;

    public async Task<IEnumerable<Category>> GetItemsAsync()
    {
        var categories = new List<Category>();

        for (int i = 1; i <= 2; i++)
        {
            var category = await _cache.StringGetAsync($"Category:{i}");
            if (!category.IsNull)
            {
                categories.Add(JsonConvert.DeserializeObject<Category>(category));
            }
        }

        return categories;
    }

    public Task<Category> GetByIdAsync(int? id)
    {
        if (id.HasValue)
        {
            var category = _cache.StringGet($"Category:{id}");
            if (!category.IsNullOrEmpty)
            {
                return Task.FromResult(JsonConvert.DeserializeObject<Category>(category));
            }
        }

        return null;
    }

    public async Task<Category> CreateAsync(Category entity)
    {
        var serializedCategory = JsonConvert.SerializeObject(entity);
        await _cache.StringSetAsync($"Category:{entity.Id}", serializedCategory);
        return entity;
    }

    public async Task<Category> UpdateAsync(Category entity)
    {
        var category = GetByIdAsync(entity.Id);
        if (category != null)
        {
            var serializedCategory = JsonConvert.SerializeObject(entity);
            await _cache.StringSetAsync($"Category:{entity.Id}", serializedCategory);
            return entity;
        }

        return null;
    }

    public async Task<Category> DeleteAsync(Category entity)
    {
        var category = await GetByIdAsync(entity.Id);
        if (category != null)
        {
            await _cache.KeyDeleteAsync($"Category:{entity.Id}");
        }

        return category;
    }
}
