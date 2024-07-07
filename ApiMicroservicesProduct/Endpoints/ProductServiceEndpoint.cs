﻿using ApiMicroservicesProduct.Dtos;
using ApiMicroservicesProduct.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ApiMicroservicesProduct.Endpoints;

public static class ProductServiceEndpoint
{
    private static async Task<T> GetCachedData<T>(IDistributedCache cache, string key)
    {
        var cachedData = await cache.GetStringAsync(key);
        return
            !string.IsNullOrEmpty(cachedData)
            ? JsonConvert.DeserializeObject<T>(cachedData)
            : default;
    }

    private static async Task SetCachedData<T>(IDistributedCache cache, string key, T data, TimeSpan expiration)
    {
        var serializedData = JsonConvert.SerializeObject(data);
        await cache.SetStringAsync(key, serializedData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        });
    }

    public static void MapProductServiceEndpoint(this WebApplication app)
    {
        app.MapGet("/api/v1/products", async (
            [FromServices] IProductDtoService service,
            IDistributedCache cache) =>
        {
            var cachedProducts = await GetCachedData<List<ProductDto>>(cache, "cached_products");

            if (cachedProducts == null)
            {
                var products = await service.GetItemsDtoAsync();
                if (products == null || !products.Any())
                    return Results.NotFound("No products found.");

                await SetCachedData(cache, "cached_products", products, TimeSpan.FromSeconds(3600));
                return Results.Ok(products);
            }

            return Results.Ok(cachedProducts);
        });

        app.MapGet("/api/v1/products/{id}", async (
            [FromServices] IProductDtoService service,
            IDistributedCache cache,
            int? id) =>
        {
            var cachedProduct = await GetCachedData<ProductDto>(cache, $"product_{id}");
            if (cachedProduct == null)
            {
                var product = await service.GetByIdAsync(id);
                if (product == null)
                    return Results.NotFound("Product not found.");

                await SetCachedData(cache, $"product_{id}", product, TimeSpan.FromSeconds(3600));
                return Results.Ok(product);
            }

            return Results.Ok(cachedProduct);
        });

        app.MapGet("/api/v1/products/search/{keyword}", async (
            [FromServices] IProductDtoService service,
            IDistributedCache cache,
            string keyword) =>
        {
            var cacheKey = $"cached_products_{keyword}";
            var cachedProducts = await GetCachedData<List<ProductDto>>(cache, cacheKey);

            if (cachedProducts == null)
            {
                IEnumerable<ProductDto> products;
                if (string.IsNullOrEmpty(keyword))
                {
                    products = await service.GetItemsDtoAsync();
                }
                else
                {
                    products = await service.GetSearchProductsDtoAsync(keyword);
                    if (!products.Any())
                        return Results.NotFound("No products found.");

                    await SetCachedData(cache, cacheKey, products, TimeSpan.FromSeconds(3600));
                }

                return Results.Ok(products);
            }

            return Results.Ok(cachedProducts);
        });

        app.MapPost("/api/v1/products", async (
            [FromServices] IProductDtoService service,
            IDistributedCache cache,
            [FromBody] ProductDto productDto,
            [FromServices] IValidator<ProductDto> validator) =>
        {
            if (productDto == null)
                return Results.BadRequest("Invalid product data.");

            var validationResult = await validator.ValidateAsync(productDto);
            var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

            if (!validationResult.IsValid)
                return Results.BadRequest(errors);

            try
            {
                await service.AddAsync(productDto);
                await cache.RemoveAsync("cached_products");

                return Results.Created($"/api/v1/products/{productDto.Id}", productDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"An error ocurred while adding the product: {ex.Message}");
            }
        });

        app.MapPut("/api/v1/products/{id}", async (
            [FromServices] IProductDtoService service,
            IDistributedCache cache,
            int? id,
            [FromBody] ProductDto productDto,
            [FromServices] IValidator<ProductDto> validator) =>
        {
            if (id == null || id != productDto.Id)
                return Results.BadRequest("Id mismatch between URL and product data.");

            var validationResult = await validator.ValidateAsync(productDto);
            var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

            if (!validationResult.IsValid)
                return Results.BadRequest(errors);

            if (productDto == null)
                return Results.BadRequest("Invalid product data.");

            try
            {
                await service.UpdateAsync(productDto);
                await cache.RemoveAsync($"product_{id}");
                await cache.RemoveAsync("cached_products");

                return Results.Ok(productDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"An error ocurred while updating the product: {ex.Message}");
            }
        });

        app.MapDelete("/api/v1/products/{id}", async (
            [FromServices] IProductDtoService service,
            IDistributedCache cache,
            int? id) =>
        {
            if (id == null)
                return Results.BadRequest("Invalid product id.");

            try
            {
                await service.DeleteAsync(id.Value);
                await cache.RemoveAsync($"product_{id}");
                await cache.RemoveAsync("cached_products");

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"An error ocurred while deleting the product: {ex.Message}");
            }
        });
    }
}
