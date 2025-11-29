using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBlue.Application.UseCases.Commons
{
    public class CustomResponse<T>(int statusCode, string message, T? data = default)
    {
        public int statusCode = statusCode;
        public string message = message;
        public T? data = data;
    }
}