using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.UseCases.Commons;
using MediatR;

namespace DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand
{
    public class CreateContactCommand : IRequest<CustomResponse<ContactDto>>
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
    }
}