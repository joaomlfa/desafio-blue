using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;

namespace DesafioBlue.Infrastructure.Persistence.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private const int DefaultPageSize = 50;
        private const int MaxPageSize = 100;
        protected BaseRepository() { }

        public int GetPageSize(int pageSize)
        {
            int size = pageSize > 0 ?
                        pageSize > MaxPageSize ?
                        MaxPageSize
                        : pageSize
                   : DefaultPageSize;
            return size + 1;
        }


        public ListPaginated<TAny> GetPaginatedResult<TAny>(List<TAny> items, int pageSize) where TAny : class
        {
            bool hasNext = items.Count > pageSize;
            if (hasNext)
            {
                items.RemoveAt(pageSize);
            }

            return new()
            {
                Items = items,
                HasNext = hasNext,
            };
        }
    }
}