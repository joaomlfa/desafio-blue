using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;
using MediatR;

namespace DesafioBlue.Application.UseCases.Contact.Command.DeleteContactCommand
{
    public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, CustomResponse<bool>>
    {
        private readonly IContactRepository _contactRepository;

        public DeleteContactHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<CustomResponse<bool>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            return await _contactRepository.DeleteContactAsync(request.Id);
        }
    }
}