using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBlue.Application.Dto;
using DesafioBlue.Application.UseCases.Commons;
using DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Domain.Entity;
using Moq;
using Xunit;

namespace DesafioBlue.Tests.Tests;

public class CreateContactHandlerTests
{
    private readonly IMapper _mapper;

    public CreateContactHandlerTests()
    {
        var cfg = new MapperConfiguration(cfg => cfg.AddProfile<DesafioBlue.Application.UseCases.Commons.MappingProfile>());
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task Handle_CreatesContact_MapsToDto()
    {
        // arrange
        var repoMock = new Mock<IContactRepository>();
        var contact = new Contact { Id = 1, Name = "John", Email = "a@b.com", Phone = "123" };
        var response = new CustomResponse<Contact>(201, "Created", contact);
        repoMock.Setup(r => r.CreateContactAsync(It.IsAny<Contact>())).ReturnsAsync(response);

        var handler = new CreateContactHandler(repoMock.Object, _mapper);

        var command = new CreateContactCommand { Name = "John", Email = "a@b.com", Phone = "123" };

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        Assert.NotNull(result);
        Assert.Equal(201, result.statusCode);
        Assert.NotNull(result.data);
        Assert.Equal("John", result.data.Name);
    }
}
