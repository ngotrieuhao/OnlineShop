using FluentValidation;
using System;

namespace OnlineShop.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            // Đây là một phương thức của abstract validator
            RuleFor(x => x.Name).NotEmpty().WithMessage("Customer name cannot be empty")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format is not correct");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number can not be empty")
                .MaximumLength(12).WithMessage("Phone number cannot exceed 12 characters");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username cannot be empty");

            RuleFor(x => x.Address).NotEmpty().WithMessage("Address cannot be left empty")
                .MaximumLength(500).WithMessage("The address cannot exceed 500 characters");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can not be empty")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$")
                .WithMessage("Password must include Uppercase, Lowercase letters and a Number");
            
            RuleFor(x => x).Custom((request, context) =>
              {
                  if (request.Password != request.ConfirmPassword)
                  {
                      context.AddFailure("Confirm password does not match password");
                  }
              });
        }
    }
}