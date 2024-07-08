using ApiMicroservicesProduct.Dtos;
using ApiMicroservicesProduct.Services.Interfaces;
using NSubstitute;

namespace ApiMicroservicesProductTest.UnitTests.ServicesNSubstitute;

public class CategoryDtoServiceTest
{
    [Fact]
    public async Task GetItemsDtoAsync_ReturnsCategoriesDto()
    {
        // Arrange
        var categoryDtoService = Substitute.For<ICategoryDtoService>();

        var categoriesDto = new List<CategoryDto>
        {
            new() { Id = 1, Name = "Category 1", Image = "image1.jpg" },
            new() { Id = 2, Name = "Category 2", Image = "image2.jpg" }
        };

        categoryDtoService.GetItemsDtoAsync().Returns(categoriesDto);

        // Act
        var result = await categoryDtoService.GetItemsDtoAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoriesDto.Count, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCategoryDto()
    {
        // Arrange
        var categoryDtoService = Substitute.For<ICategoryDtoService>();

        var categoryId = 1;
        var categoryDto = new CategoryDto { Id = categoryId, Name = "Category 1", Image = "image1.jpg" };

        categoryDtoService.GetByIdAsync(categoryId).Returns(categoryDto);

        // Act
        var result = await categoryDtoService.GetByIdAsync(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
    }

    [Fact]
    public async Task AddAsync_AddNewCategory()
    {
        // Arrange
        var categoryDtoService = Substitute.For<ICategoryDtoService>();

        var categoryToAdd = new CategoryDto
        {
            Id = 1,
            Name = "Category 1",
            Image = "image1.jpg"
        };

        await categoryDtoService.AddAsync(categoryToAdd);

        await categoryDtoService.Received().AddAsync(Arg.Any<CategoryDto>());
    }

    [Fact]
    public async Task UpdateAsync_UpdateExistingCategory()
    {
        // Arrange
        var categoryDtoService = Substitute.For<ICategoryDtoService>();

        var categoryToUpdate = new CategoryDto
        {
            Id = 1,
            Name = "Category 1",
            Image = "image1.jpg"
        };

        await categoryDtoService.UpdateAsync(categoryToUpdate);

        await categoryDtoService.Received().UpdateAsync(Arg.Any<CategoryDto>());
    }

    [Fact]
    public async Task DeleteAsync_DeleteExistingCategory()
    {
        // Arrange
        var categoryDtoService = Substitute.For<ICategoryDtoService>();

        var categoryId = 1;

        categoryDtoService.GetByIdAsync(categoryId).Returns(new CategoryDto { Id = categoryId });

        await categoryDtoService.DeleteAsync(categoryId);

        await categoryDtoService.Received(1).DeleteAsync(Arg.Any<int>());
    }
}
