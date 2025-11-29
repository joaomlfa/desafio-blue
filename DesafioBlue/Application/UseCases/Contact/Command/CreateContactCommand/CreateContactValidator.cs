using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand
{
    public class CreateContactValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone must be a valid international phone number.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");
        }
    }
}