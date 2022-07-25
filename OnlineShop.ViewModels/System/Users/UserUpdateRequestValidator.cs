using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.System.Users
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
               .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format is not correct");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
        }
    }
}