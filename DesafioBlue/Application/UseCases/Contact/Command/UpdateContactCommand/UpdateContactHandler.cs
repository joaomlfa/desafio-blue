using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;
using MediatR;

namespace DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand
{
    public class UpdateContactHandler(IMapper mapper, IContactRepository contactRepository) : IRequestHandler<UpdateContactCommand, CustomResponse<ContactDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IContactRepository _contactRepository = contactRepository;

        public async Task<CustomResponse<ContactDto>> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var result = await _contactRepository.UpdateContactAsync(_mapper.Map<Domain.Entity.Contact>(request));
            return _mapper.Map<CustomResponse<ContactDto>>(result);
        }
    }
}