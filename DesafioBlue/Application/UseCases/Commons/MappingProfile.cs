using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand;

namespace DesafioBlue.Application.UseCases.Commons
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entity.Contact, Dto.ContactDto>();
            CreateMap<CreateContactCommand, Domain.Entity.Contact>();
            CreateMap<UpdateContactCommand, Domain.Entity.Contact>();
            CreateMap<ListPaginated<Domain.Entity.Contact>, ListPaginated<Dto.ContactDto>>();
            CreateMap<CustomResponse<Domain.Entity.Contact>, CustomResponse<Dto.ContactDto>>();
        }
    }
}