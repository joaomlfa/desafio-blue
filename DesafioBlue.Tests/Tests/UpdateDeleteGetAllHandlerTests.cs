using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;
using DesafioBlue.Application.UseCases.Contact.Command.DeleteContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Query.GetAllQuery;
using DesafioBlue.Domain.Entity;
using Moq;
using Xunit;

namespace DesafioBlue.Tests.Tests;

public class UpdateDeleteGetAllHandlerTests
{
    private readonly IMapper _mapper;

    public UpdateDeleteGetAllHandlerTests()
    {
        var cfg = new MapperConfiguration(cfg => cfg.AddProfile<DesafioBlue.Application.UseCases.Commons.MappingProfile>());
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task UpdateContactHandler_MapsAndReturns()
    {
        var repoMock = new Mock<IContactRepository>();
        var contact = new Contact { Id = 1, Name = "X", Email = "e@e.com", Phone = "p" };
        repoMock.Setup(r => r.UpdateContactAsync(It.IsAny<Contact>())).ReturnsAsync(new CustomResponse<Contact>(200, "Updated", contact));

        var handler = new DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand.UpdateContactHandler(_mapper, repoMock.Object);
        var cmd = new UpdateContactCommand { Id = 1, Name = "X", Email = "e@e.com", Phone = "p" };

        var res = await handler.Handle(cmd, CancellationToken.None);
        Assert.Equal(200, res.statusCode);
        Assert.Equal("X", res.data.Name);
    }

    [Fact]
    public async Task DeleteContactHandler_ReturnsResult()
    {
        var repoMock = new Mock<IContactRepository>();
        repoMock.Setup(r => r.DeleteContactAsync(1)).ReturnsAsync(new CustomResponse<bool>(200, "Deleted", true));

        var handler = new DesafioBlue.Application.UseCases.Contact.Command.DeleteContactCommand.DeleteContactHandler(repoMock.Object);
        var cmd = new DeleteContactCommand { Id = 1 };

        var res = await handler.Handle(cmd, CancellationToken.None);
        Assert.Equal(200, res.statusCode);
        Assert.True(res.data);
    }

    [Fact]
    public async Task GetAllContactHandler_ReturnsMappedPaginated()
    {
        var repoMock = new Mock<IContactRepository>();
        var contacts = new List<Contact> { new Contact { Id = 1, Name = "A", Email = "a@b.com", Phone = "p" } };
        var paginated = new ListPaginated<Contact> { HasNext = false, Items = contacts };
        repoMock.Setup(r => r.GetAllContactsPaginatedAsync(50, 0)).ReturnsAsync(new CustomResponse<ListPaginated<Contact>>(200, "Ok", paginated));

        var handler = new DesafioBlue.Application.UseCases.Contact.Query.GetAllQuery.GetAllContactHandler(repoMock.Object, _mapper);
        var query = new GetAllContactQuery { PageSize = 50, CursorAfter = 0 };

        var res = await handler.Handle(query, CancellationToken.None);
        Assert.Equal(200, res.statusCode);
        Assert.NotNull(res.data);
        Assert.Equal(1, res.data.Items.Count);
    }
}
