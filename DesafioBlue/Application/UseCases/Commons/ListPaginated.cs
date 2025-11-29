using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBlue.Application.UseCases.Commons
{
    public class ListPaginated<T>
    {
        public bool HasNext { get; set; }
        public required IList<T> Items { get; set; }

    }
}