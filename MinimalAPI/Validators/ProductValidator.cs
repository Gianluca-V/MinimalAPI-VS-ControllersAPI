using FluentValidation;
using MinimalAPI.Models;

namespace MinimalAPI.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() 
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name cant be null.");
            RuleFor(p => p.Price).GreaterThanOrEqualTo(0).Must(p => p%1 == 0).NotEmpty().WithMessage("Price must be a positive number without decimals.");
            RuleFor(p => p.Stock).GreaterThanOrEqualTo(0).Must(p => p % 1 == 0).NotEmpty().WithMessage("Stock must be a positive number without decimals.");
        }
    }
}
