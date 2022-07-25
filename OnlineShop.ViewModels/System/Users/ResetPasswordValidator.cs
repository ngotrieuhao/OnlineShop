using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.System.Users
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can not be empty")
                 .MinimumLength(8).WithMessage("Password must be at least 8 characters")
               .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$")
               .WithMessage("Password must include Uppercase, Lowercase letters and a Number");
        }
    }
}