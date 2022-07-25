using FluentValidation;
using System;

namespace OnlineShop.ViewModels.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            // Đây là một phương thức của abstract validator
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username cannot be empty");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can not be empty");
        }
    }
}