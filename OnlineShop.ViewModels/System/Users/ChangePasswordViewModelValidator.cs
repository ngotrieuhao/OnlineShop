using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.System.Users
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password can not be empty")
                 .MinimumLength(8).WithMessage("Password must be at least 8 characters")
               .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$")
               .WithMessage("Password must include Uppercase, Lowercase letters and a Number");
        }
    }
}