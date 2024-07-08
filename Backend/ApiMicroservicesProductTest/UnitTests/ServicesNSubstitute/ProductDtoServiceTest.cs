using ApiMicroservicesProduct.Dtos;
using ApiMicroservicesProduct.Services.Interfaces;
using NSubstitute;

namespace ApiMicroservicesProductTest.UnitTests.ServicesNSubstitute;

public class ProductDtoServiceTest
{
    private static readonly string[] sourceArray1 = ["image1.jpg", "image2.jpg"];
    private static readonly string[] sourceArray2 = ["image3.jpg", "image4.jpg"];

    [Fact]
    public async Task GetItemsDtoAsync_ReturnsProductsDto()
    {
        // Arrange
        var productDtoService = Substitute.For<IProductDtoService>();

        var productsDto = new List<ProductDto>
        {
            new()
            {
                Id = 1,
                Name = "Product 1",
                Images = [.. sourceArray1],
                Description = "Description 1",
                Price = 10.0M,
                Stock = 10,
                CategoryId = 1
            },
            new()
            {
                Id = 2,
                Name = "Product 2",
                Images = [.. sourceArray2],
                Description = "Description 2",
                Price = 20.0M,
                Stock = 20,
                CategoryId = 2
            }
        };

        productDtoService.GetItemsDtoAsync().Returns(productsDto);

        // Act
        var result = await productDtoService.GetItemsDtoAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productsDto.Count, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnProductDto()
    {
        // Arrange
        var productDtoService = Substitute.For<IProductDtoService>();
        var productId = 1;

        var productDto = new ProductDto
        {
            Id = 1,
            Name = "Product 1",
            Images = [.. sourceArray1],
            Description = "Description 1",
            Price = 10.0M,
            Stock = 10,
            CategoryId = 1
        };

        productDtoService.GetByIdAsync(productId).Returns(productDto);

        // Act
        var result = await productDtoService.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }

    [Fact]
    public async Task AddAsync_AddNewProduct()
    {
        // Arrange
        var productDtoService = Substitute.For<IProductDtoService>();

        var productToAdd = new ProductDto
        {
            Id = 1,
            Name = "Product 1",
            Images = [.. sourceArray1],
            Description = "Description 1",
            Price = 10.0M,
            Stock = 10,
            CategoryId = 1
        };

        await productDtoService.AddAsync(productToAdd);

        await productDtoService.Received().AddAsync(Arg.Any<ProductDto>());
    }

    [Fact]
    public async Task UpdateAsync_UpdateExistingProduct()
    {
        // Arrange
        var productDtoService = Substitute.For<IProductDtoService>();

        var productToUpdate = new ProductDto
        {
            Id = 1,
            Name = "Product 1",
            Images = [.. sourceArray1],
            Description = "Description 1",
            Price = 10.0M,
            Stock = 10,
            CategoryId = 1
        };

        await productDtoService.UpdateAsync(productToUpdate);

        await productDtoService.Received().UpdateAsync(Arg.Any<ProductDto>());
    }
}
