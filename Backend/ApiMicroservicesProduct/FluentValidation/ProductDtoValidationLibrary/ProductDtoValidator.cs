using ApiMicroservicesProduct.Dtos;
using FluentValidation;

namespace ApiMicroservicesProduct.FluentValidation.ProductDtoValidationLibrary;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .Length(10, 5000).WithMessage("Product description must be between 10 and 5000 characters.");

        RuleForEach(x => x.Images)
            .Must(x => x.Length <= 600)
            .WithMessage("Each image should have a maximum length of 600 characters.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Product price is required.")
            .InclusiveBetween(1, 999).WithMessage("Product price must be between 1 and 999.");

        RuleFor(x => x.Stock)
            .NotEmpty().WithMessage("Product stock is required.")
            .InclusiveBetween(1, 99).WithMessage("Stock must be between 1 and 99.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.")
            .GreaterThan(0).WithMessage("Category ID must be greater than 0.");
    }
}
