using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.UseCases.Commons;

namespace DesafioBlue.Application.Interface.Persistence.Repository
{
    public interface IBaseRepository
    {
        int GetPageSize(int pageSize);
        ListPaginated<TAny> GetPaginatedResult<TAny>(List<TAny> items, int pageSize) where TAny : class;
    }
}