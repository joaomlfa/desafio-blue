using System.Threading.Tasks;
using DesafioBlue.Application.UseCases.Contact.Command.CreateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.DeleteContactCommand;
using DesafioBlue.Application.UseCases.Contact.Command.UpdateContactCommand;
using DesafioBlue.Application.UseCases.Contact.Query.GetAllQuery;
using FluentValidation.Results;
using Xunit;

namespace DesafioBlue.Tests.Tests;

public class ValidatorTests
{
    [Fact]
    public void CreateContactValidator_InvalidPhoneAndEmail_Fails()
    {
        var validator = new CreateContactValidator();
        var cmd = new CreateContactCommand { Name = "", Phone = "abc", Email = "not-an-email" };
        var result = validator.Validate(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        Assert.Contains(result.Errors, e => e.PropertyName == "Phone");
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void UpdateContactValidator_InvalidIdAndPhone_Fails()
    {
        var validator = new UpdateContactValidator();
        var cmd = new UpdateContactCommand { Id = 0, Name = "", Email = "bad", Phone = "123456" };
        var result = validator.Validate(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Id");
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void DeleteContactValidator_InvalidId_Fails()
    {
        var validator = new DeleteContactValidator();
        var cmd = new DeleteContactCommand { Id = 0 };
        var result = validator.Validate(cmd);
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void GetAllContactValidator_InvalidPageSize_Fails()
    {
        var validator = new GetAllContactValidator();
        var q = new GetAllContactQuery { PageSize = 0, CursorAfter = 0 };
        var result = validator.Validate(q);
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void CreateContactValidator_ValidInput_Passes()
    {
        var validator = new CreateContactValidator();
        var cmd = new CreateContactCommand { Name = "Valid Name", Phone = "+1234567890", Email = "valid@example.com" };
        var result = validator.Validate(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void UpdateContactValidator_ValidInput_Passes()
    {
        var validator = new UpdateContactValidator();
        var cmd = new UpdateContactCommand { Id = 1, Name = "Valid Name", Email = "valid@example.com", Phone = "+1234567890" };
        var result = validator.Validate(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void DeleteContactValidator_ValidId_Passes()
    {
        var validator = new DeleteContactValidator();
        var cmd = new DeleteContactCommand { Id = 5 };
        var result = validator.Validate(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void GetAllContactValidator_ValidPageSize_Passes()
    {
        var validator = new GetAllContactValidator();
        var q = new GetAllContactQuery { PageSize = 10, CursorAfter = 0 };
        var result = validator.Validate(q);
        Assert.True(result.IsValid);
    }
}
