using Dealio.Core.Features.Product.Commands.Models;
using FluentValidation;

namespace Dealio.Core.Features.Product.Commands.Validation
{
    public class UpdateProductComandValidation : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductComandValidation()
        {
            RuleFor(r => r.ProductId)
                .NotEmpty().WithMessage("Product ID is required.")
                .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MinimumLength(3).WithMessage("Product name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MinimumLength(10).WithMessage("Product description must be at least 10 characters long.")
                .MaximumLength(500).WithMessage("Product description must not exceed 500 characters.");

            RuleFor(r => r.Price)
                .NotEmpty().WithMessage("Product price is required.")
                .GreaterThan(0).WithMessage("Product price must be greater than 0.");

            RuleFor(r => r.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.")
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.");


        }
    }
}
