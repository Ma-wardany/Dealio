using Dealio.Core.Features.ApplicationUser.Commands.Models;
using FluentValidation;
using System;

namespace Dealio.Core.Features.ApplicationUser.Commands.Validtion
{
    public class RegisterCommandValidation : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidation()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(r => r.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");

            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(r => r.Password).WithMessage("The password and confirmation password do not match.");
        }
    }
}
