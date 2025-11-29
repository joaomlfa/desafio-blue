using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;
using MediatR;

namespace DesafioBlue.Application.UseCases.Contact.Query.GetAllQuery
{
    public class GetAllContactHandler : IRequestHandler<GetAllContactQuery, CustomResponse<ListPaginated<ContactDto>>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public GetAllContactHandler(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<CustomResponse<ListPaginated<ContactDto>>> Handle(GetAllContactQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _contactRepository.GetAllContactsPaginatedAsync(request.PageSize, request.CursorAfter);
            return new CustomResponse<ListPaginated<ContactDto>>(contacts.statusCode, contacts.message, _mapper.Map<ListPaginated<ContactDto>>(contacts.data));
        }
    }
}