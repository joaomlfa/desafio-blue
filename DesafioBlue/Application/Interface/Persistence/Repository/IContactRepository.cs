using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.UseCases.Commons;
using DesafioBlue.Domain.Entity;

namespace DesafioBlue.Application.Interface.Persistence.Repository
{
    public interface IContactRepository
    {
        Task<CustomResponse<ListPaginated<Contact>>> GetAllContactsPaginatedAsync(int pageSize, int cursorAfter);
        Task<CustomResponse<Contact>> CreateContactAsync(Contact contact);
        Task<CustomResponse<Contact>> UpdateContactAsync(Contact contact);
        Task<CustomResponse<bool>> DeleteContactAsync(int id);
    }
}
