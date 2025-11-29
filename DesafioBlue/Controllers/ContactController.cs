using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.UseCases.Commons;
using DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.DeleteContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Query.GetAllQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DesafioBlue.Controllers
{
    public class ContactController : ControllerBaseDesafioBlue
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IMediator _mediator;

        public ContactController(ILogger<ContactController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/v1/[controller]")]
        [ProducesResponseType<ListPaginated<ContactDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListPaginated<ContactDto>>> GetAllContacts(GetAllContactQuery query)
        {
            var response = await _mediator.Send(query);
            return ProcessResponse(response);
        }

        [HttpPost]
        [Route("api/v1/[controller]")]
        [ProducesResponseType<ListPaginated<ContactDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactDto>> CreateContact(CreateContactCommand command)
        {
            var response = await _mediator.Send(command);
            return ProcessResponse(response);
        }

        [HttpDelete]
        [Route("api/v1/[controller]")]
        [ProducesResponseType<ListPaginated<ContactDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteContact(DeleteContactCommand command)
        {
            var response = await _mediator.Send(command);
            return ProcessResponse(response);
        }

        [HttpPatch]
        [Route("api/v1/[controller]")]
        [ProducesResponseType<ListPaginated<ContactDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactDto>> UpdateContact(UpdateContactCommand command)
        {
            var response = await _mediator.Send(command);
            return ProcessResponse(response);
        }
    }
}