using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;
using DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand;
using Moq;
using Xunit;

namespace DesafioBlue.Tests.Tests;

public class HandlerErrorScenariosTests
{
    private readonly IMapper _mapper;

    public HandlerErrorScenariosTests()
    {
        var cfg = new MapperConfiguration(cfg => cfg.AddProfile<DesafioBlue.Application.UseCases.Commons.MappingProfile>());
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task CreateContactHandler_WhenRepoReturnsServerError_Propagates()
    {
        var repoMock = new Mock<IContactRepository>();
        repoMock.Setup(r => r.CreateContactAsync(It.IsAny<Domain.Entity.Contact>()))
            .ReturnsAsync(new CustomResponse<Domain.Entity.Contact>(500, "error", null));

        var handler = new CreateContactHandler(repoMock.Object, _mapper);
        var cmd = new CreateContactCommand { Name = "A", Email = "a@b.com", Phone = "+12345" };

        var res = await handler.Handle(cmd, CancellationToken.None);
        Assert.Equal(500, res.statusCode);
        Assert.Null(res.data);
    }

    [Fact]
    public async Task UpdateContactHandler_WhenNotFound_Returns404()
    {
        var repoMock = new Mock<IContactRepository>();
        repoMock.Setup(r => r.UpdateContactAsync(It.IsAny<Domain.Entity.Contact>()))
            .ReturnsAsync(new CustomResponse<Domain.Entity.Contact>(404, "not found", null));

        var handler = new DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand.UpdateContactHandler(_mapper, repoMock.Object);
        var cmd = new UpdateContactCommand { Id = 999, Name = "X", Email = "x@x.com", Phone = "+12345" };

        var res = await handler.Handle(cmd, CancellationToken.None);
        Assert.Equal(404, res.statusCode);
        Assert.Null(res.data);
    }
}
