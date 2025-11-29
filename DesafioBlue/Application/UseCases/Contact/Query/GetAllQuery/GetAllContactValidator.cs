using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace DesafioBlue.Application.UseCases.Contact.Query.GetAllQuery
{
    public class GetAllContactValidator : AbstractValidator<GetAllContactQuery>
    {
        public GetAllContactValidator()
        {

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }
}