using ApiMicroservicesProduct.Models;
using ApiMicroservicesProduct.Models.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ApiMicroservicesProductTest.IntegrationTests.Categories;

public class RedisCategoryCacheDirectAccessTests
{
    private ICategoryRepository GetRedisRepository()
    {
        var redis = ConnectionMultiplexer.Connect("localhost:6379");
        var cache = redis.GetDatabase();
        return new RedisCategoryRepository(cache);
    }

    private async Task AddCategoriesToRedis(params Category[] categories)
    {
        using var redis = ConnectionMultiplexer.Connect("localhost:6379");
        var cache = redis.GetDatabase();

        foreach (var category in categories)
        {
            var serializedCategory = JsonConvert.SerializeObject(category);
            await cache.StringSetAsync($"Category:{category.Id}", serializedCategory);
        }
    }

    private async Task ClearCategoryFromRedis(int categoryId)
    {
        using var redis = ConnectionMultiplexer.Connect("localhost:6379");
        var cache = redis.GetDatabase();

        await cache.KeyDeleteAsync($"Category:{categoryId}");
    }

    [Fact]
    public async Task GetItemsAsync_ReturnCategoriesFromRedis()
    {
        var categoryTest1 = new Category(1, "Test1", "image1.png");
        var categoryTest2 = new Category(2, "Test2", "image2.png");

        await AddCategoriesToRedis(categoryTest1, categoryTest2);

        var repository = GetRedisRepository();
        var categories = await repository.GetItemsAsync();

        Assert.NotNull(categories);
        Assert.Equal(2, categories.Count());

        await ClearCategoryFromRedis(categoryTest1.Id);
        await ClearCategoryFromRedis(categoryTest2.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnCorrectCategory()
    {
        var repository = GetRedisRepository();
        var category = await repository.GetByIdAsync(3);

        Assert.NotNull(category);
        Assert.Equal(3, category.Id);
        Assert.Equal("Test3", category.Name);
        Assert.Equal("image3.png", category.Image);
    }

    [Fact]
    public async Task CreateAsync_AddCategoryToRedis()
    {
        var category = new Category(3, "Test3", "image3.png");

        var repository = GetRedisRepository();
        var createdCategory = await repository.CreateAsync(category);

        Assert.NotNull(createdCategory);
        Assert.Equal(3, createdCategory.Id);
        Assert.Equal("Test3", createdCategory.Name);
        Assert.Equal("image3.png", createdCategory.Image);
    }

    [Fact]
    public async Task UpdateAsync_UpdateCategoryInRedis()
    {
        var repository = GetRedisRepository();
        var categoryToUpdate = await repository.GetByIdAsync(3);
        var updatedCategory = new Category(categoryToUpdate.Id, "Update Test 3", categoryToUpdate.Image);

        await repository.UpdateAsync(updatedCategory);

        var retrievedUpdateCategory = await repository.GetByIdAsync(3);

        Assert.NotNull(retrievedUpdateCategory);
        Assert.Equal(3, retrievedUpdateCategory.Id);
        Assert.Equal("Update Test 3", retrievedUpdateCategory.Name);
        Assert.Equal("image3Update.png", retrievedUpdateCategory.Image);
    }

    [Fact]
    public async Task DeleteAsync_RemoveCategoryFromRedis()
    {
        var repository = GetRedisRepository();
        var categoryToRemove = await repository.GetByIdAsync(3);
        var removedCategory = await repository.DeleteAsync(categoryToRemove);

        var deletedCategory = await repository.GetByIdAsync(3);

        Assert.NotNull(removedCategory);
        Assert.Null(deletedCategory);
    }
}
