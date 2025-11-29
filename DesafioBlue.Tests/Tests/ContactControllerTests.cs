using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.UseCases.Commons;
using DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.DeleteContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Query.GetAllQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DesafioBlue.Tests.Tests;

public class ContactControllerTests
{
    private readonly IMapper _mapper;

    public ContactControllerTests()
    {
        var cfg = new MapperConfiguration(cfg => cfg.AddProfile<DesafioBlue.Application.UseCases.Commons.MappingProfile>());
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task CreateContact_Controller_ReturnsCreated()
    {
        var mediatorMock = new Mock<IMediator>();
        var contact = new ContactDto { Name = "J", Email = "j@j.com", Phone = "p" };
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomResponse<ContactDto>(201, "Created", contact));

        var loggerMock = new Mock<ILogger<DesafioBlue.Controllers.ContactController>>();
        var controller = new DesafioBlue.Controllers.ContactController(loggerMock.Object, mediatorMock.Object);
        var cmd = new CreateContactCommand { Name = "J", Email = "j@j.com", Phone = "p" };

        var result = await controller.CreateContact(cmd);

        Assert.IsType<CreatedResult>(result.Result);
    }

    [Fact]
    public async Task GetAllContacts_Controller_ReturnsOk()
    {
        var mediatorMock = new Mock<IMediator>();
        var list = new ListPaginated<ContactDto> { HasNext = false, Items = new System.Collections.Generic.List<ContactDto>() };
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllContactQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomResponse<ListPaginated<ContactDto>>(200, "Ok", list));

        var controller = new DesafioBlue.Controllers.ContactController(new Mock<ILogger<DesafioBlue.Controllers.ContactController>>().Object, mediatorMock.Object);
        var query = new GetAllContactQuery { PageSize = 50, CursorAfter = 0 };

        var result = await controller.GetAllContacts(query);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateContact_Controller_ReturnsOk()
    {
        var mediatorMock = new Mock<IMediator>();
        var dto = new ContactDto { Id = 1, Name = "N", Email = "e@e.com", Phone = "p" };
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomResponse<ContactDto>(200, "Updated", dto));

        var controller = new DesafioBlue.Controllers.ContactController(new Mock<ILogger<DesafioBlue.Controllers.ContactController>>().Object, mediatorMock.Object);
        var cmd = new UpdateContactCommand { Id = 1, Name = "N", Email = "e@e.com", Phone = "p" };

        var result = await controller.UpdateContact(cmd);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteContact_Controller_ReturnsOk()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomResponse<bool>(200, "Deleted", true));

        var controller = new DesafioBlue.Controllers.ContactController(new Mock<ILogger<DesafioBlue.Controllers.ContactController>>().Object, mediatorMock.Object);
        var cmd = new DeleteContactCommand { Id = 1 };

        var result = await controller.DeleteContact(cmd);
        Assert.IsType<OkObjectResult>(result.Result);
    }
}
