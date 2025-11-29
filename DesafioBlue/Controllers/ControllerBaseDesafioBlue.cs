using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.UseCases.Commons;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBlue.Controllers
{
    public class ControllerBaseDesafioBlue : ControllerBase
    {
        public ActionResult<T> ProcessResponse<T>(CustomResponse<T> response)
        {
            return response.statusCode switch
            {
                StatusCodes.Status200OK => new OkObjectResult(response.data),
                StatusCodes.Status404NotFound => new NotFoundObjectResult(new { erro = response.message }),
                StatusCodes.Status201Created => new CreatedResult(string.Empty, response.data),
                _ => new ObjectResult(new { erro = response.message }) { StatusCode = response.statusCode }
            };
        }
    }
}