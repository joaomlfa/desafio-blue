using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.UseCases.Commons;
using MediatR;

namespace DesafioBlue.Application.UseCases.Contact.Command.DeleteContactCommand
{
    public class DeleteContactCommand : IRequest<CustomResponse<bool>>
    {
        public required int Id { get; set; }
    }
}