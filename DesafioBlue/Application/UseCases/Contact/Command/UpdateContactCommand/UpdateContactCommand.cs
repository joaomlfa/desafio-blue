using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.UseCases.Commons;
using MediatR;

namespace DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand
{
    public class UpdateContactCommand : IRequest<CustomResponse<ContactDto>>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

    }
}