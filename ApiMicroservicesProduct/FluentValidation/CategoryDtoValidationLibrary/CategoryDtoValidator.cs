using ApiMicroservicesProduct.Dtos;
using FluentValidation;

namespace ApiMicroservicesProduct.FluentValidation.CategoryDtoValidationLibrary;

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .Length(3, 100).WithMessage("Category name must be between 3 and 100 characters.");

        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Image is required.")
            .MaximumLength(600).WithMessage("Image must be less than 600 characters.");
    }
}