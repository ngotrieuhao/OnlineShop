using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.Sales
{
    public class CheckoutRequestViewModelValidation : AbstractValidator<CheckoutRequest>
    {
        public CheckoutRequestViewModelValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.Address).NotEmpty().WithMessage("Address cannot be empty")
                .MaximumLength(500).WithMessage("The address cannot exceed 500 characters");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number can not be empty")
                .MaximumLength(12).WithMessage("Phone number cannot exceed 12 characters");
        }
    }
}