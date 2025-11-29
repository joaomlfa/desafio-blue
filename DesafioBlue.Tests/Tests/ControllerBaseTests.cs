using DesafioBlue.Application.UseCases.Commons;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DesafioBlue.Tests.Tests;

public class ControllerBaseTests
{
    [Fact]
    public void ProcessResponse_Returns_Ok_For_200()
    {
        var ctrl = new DesafioBlue.Controllers.ControllerBaseDesafioBlue();
        var response = new CustomResponse<string>(200, "Ok", "payload");
        var result = ctrl.ProcessResponse(response);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void ProcessResponse_Returns_NotFound_For_404()
    {
        var ctrl = new DesafioBlue.Controllers.ControllerBaseDesafioBlue();
        var response = new CustomResponse<string>(404, "not found", null);
        var result = ctrl.ProcessResponse(response);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void ProcessResponse_Returns_Created_For_201()
    {
        var ctrl = new DesafioBlue.Controllers.ControllerBaseDesafioBlue();
        var response = new CustomResponse<string>(201, "created", "p");
        var result = ctrl.ProcessResponse(response);
        Assert.IsType<CreatedResult>(result.Result);
    }
}
