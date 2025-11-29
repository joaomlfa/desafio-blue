using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.UseCases.Commons;
using MediatR;

namespace DesafioBlue.Application.UseCases.Contact.Query.GetAllQuery
{
    public class GetAllContactQuery : IRequest<CustomResponse<ListPaginated<ContactDto>>>
    {
        public int PageSize { get; set; } = 50;
        public int CursorAfter { get; set; } = 0;
    }
}