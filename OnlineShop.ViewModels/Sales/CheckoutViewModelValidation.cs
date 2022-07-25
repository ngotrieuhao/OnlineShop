using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.Sales
{
    public class CheckoutViewModelValidation : AbstractValidator<CheckoutViewModel>
    {
        public CheckoutViewModelValidation()
        {
            RuleFor(x => x.CheckoutModel.Name).NotEmpty().WithMessage("Name cannot be empty")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.CheckoutModel.Address).NotEmpty().WithMessage("Address cannot be empty")
                .MaximumLength(500).WithMessage("The address cannot exceed 500 characters");

            RuleFor(x => x.CheckoutModel.PhoneNumber).NotEmpty().WithMessage("Phone number can not be empty")
                .MaximumLength(12).WithMessage("Phone number cannot exceed 12 characters");
        }
    }
}