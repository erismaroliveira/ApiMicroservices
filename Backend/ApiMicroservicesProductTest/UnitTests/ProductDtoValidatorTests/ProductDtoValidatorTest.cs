using ApiMicroservicesProduct.Dtos;
using ApiMicroservicesProduct.FluentValidation.ProductDtoValidationLibrary;
using FluentValidation.TestHelper;

namespace ApiMicroservicesProductTest.UnitTests.ProductDtoValidatorTests;

public class ProductDtoValidatorTest
{
    private readonly ProductDtoValidator _validator;

    public ProductDtoValidatorTest()
    {
        _validator = new ProductDtoValidator();
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Properties_Are_Valid()
    {
        var product = new ProductDto
        {
            Name = "Product Name",
            Description = "Product Description",
            Price = 10.0m,
            Stock = 10,
            CategoryId = 1
        };

        var result = _validator.TestValidate(product);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        var product = new ProductDto
        {
            Name = null
        };

        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Length_Is_Too_Short()
    {
        var product = new ProductDto
        {
            Name = "Ab"
        };

        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Null()
    {
        var product = new ProductDto
        {
            Description = null
        };

        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Length_Is_Too_Short()
    {
        var product = new ProductDto
        {
            Description = "Ab"
        };

        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Image_Have_Valid_Length()
    {
        var product = new ProductDto
        {
            Name = "Product Name",
            Description = "Product Description",
            Price = 10.0m,
            Stock = 10,
            Images = ["image1.jpg", "image2.png"]
        };

        var result = _validator.TestValidate(product);
        result.ShouldNotHaveValidationErrorFor(x => x.Images);
    }

    [Fact]
    public void Should_Have_Error_When_Any_Image_Length_Is_Too_Long()
    {
        var stringTest = new string('a', 601);

        var product = new ProductDto
        {
            Name = "Product Name",
            Description = "Product Description",
            Price = 10.0m,
            Stock = 10,
            Images = ["image.jpg", "image1.jpg", stringTest]
        };

        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(x => x.Images);
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Outside_Ranger()
    {
        var product = new ProductDto
        {
            Price = 0
        };

        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_Have_Error_When_Stock_Is_Outside_Ranger()
    {
        var product = new ProductDto
        {
            Stock = 0
        };

        var result = _validator.TestValidate(product);
        result.ShouldHaveValidationErrorFor(x => x.Stock);
    }
}
