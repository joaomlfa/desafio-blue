using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand
{
    public class UpdateContactValidator : AbstractValidator<UpdateContactCommand>
    {
        public UpdateContactValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("A valid phone number is required.")
                .MaximumLength(15).WithMessage("Phone must not exceed 15 characters.");
        }

    }
}