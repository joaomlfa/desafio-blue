using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace DesafioBlue.Application.UseCases.Contact.Command.DeleteContactCommand
{
    public class DeleteContactValidator : AbstractValidator<DeleteContactCommand>
    {
        public DeleteContactValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}