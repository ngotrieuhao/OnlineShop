using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.Catalog.Products
{
    public class ProductCreateRequestValidation : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidation()
        {
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price cannot be empty")
                .LessThan(10000).WithMessage("Price must be less than 10.000")
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Stock).NotEmpty().WithMessage("Quantity cannot be empty")
                .LessThan(1000).WithMessage("Quantity must be less than 1,000")
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name cannot be empty")
               .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Specifications cannot be empty")
               .MaximumLength(4000).WithMessage("Specifications should not exceed 4000 characters");

            RuleFor(x => x.Details)
                .MaximumLength(10000).WithMessage("Product description should not exceed 10,000 characters");
        }
    }
}