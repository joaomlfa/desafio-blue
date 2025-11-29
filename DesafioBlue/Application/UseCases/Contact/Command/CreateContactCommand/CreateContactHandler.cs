using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;
using MediatR;

namespace DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand
{
    public class CreateContactHandler(IContactRepository contactRepository, IMapper mapper) : IRequestHandler<CreateContactCommand, CustomResponse<ContactDto>>
    {
        private readonly IContactRepository _contactRepository = contactRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<CustomResponse<ContactDto>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var result = await _contactRepository.CreateContactAsync(_mapper.Map<Domain.Entity.Contact>(request));
            return _mapper.Map<CustomResponse<ContactDto>>(result);
        }
    }
}