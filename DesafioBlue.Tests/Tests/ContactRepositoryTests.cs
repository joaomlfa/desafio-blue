using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Domain.Entity;
using DesafioBlue.Infrastructure.Persistence.Context;
using DesafioBlue.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DesafioBlue.Tests.Tests;

public class ContactRepositoryTests
{
    private ContactContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ContactContext(options);
    }

    [Fact]
    public async Task CreateContactAsync_AddsContact()
    {
        var ctx = CreateContext("create-db");
        var repo = new ContactRepository(ctx);

        var contact = new Contact { Name = "A", Email = "a@b.com", Phone = "111" };
        var res = await repo.CreateContactAsync(contact);

        Assert.Equal(201, res.statusCode);
        Assert.NotNull(res.data);
        Assert.Equal("A", res.data.Name);
        Assert.Equal(1, ctx.Contacts.Count());
    }

    [Fact]
    public async Task GetAllContactsPaginatedAsync_Returns_Paginated()
    {
        var ctx = CreateContext("paginated-db");
        // seed 5
        for (int i = 1; i <= 5; i++) ctx.Contacts.Add(new Contact { Name = i.ToString(), Email = $"{i}@x.com", Phone = "p" });
        await ctx.SaveChangesAsync();

        var repo = new ContactRepository(ctx);
        var res = await repo.GetAllContactsPaginatedAsync(2, 0);

        Assert.Equal(200, res.statusCode);
        Assert.NotNull(res.data);
        Assert.True(res.data.HasNext);
        Assert.Equal(2, res.data.Items.Count);
    }

    [Fact]
    public async Task UpdateContactAsync_Updates()
    {
        var ctx = CreateContext("update-db");
        var c = new Contact { Name = "X", Email = "x@x.com", Phone = "p" };
        ctx.Contacts.Add(c);
        await ctx.SaveChangesAsync();

        var repo = new ContactRepository(ctx);
        c.Name = "Y";
        var res = await repo.UpdateContactAsync(c);

        Assert.Equal(200, res.statusCode);
        Assert.Equal("Y", res.data.Name);
    }

    [Fact]
    public async Task DeleteContactAsync_Deletes()
    {
        var ctx = CreateContext("delete-db");
        var c = new Contact { Name = "ToDel", Email = "d@d.com", Phone = "p" };
        ctx.Contacts.Add(c);
        await ctx.SaveChangesAsync();

        var repo = new ContactRepository(ctx);
        var res = await repo.DeleteContactAsync(c.Id);

        Assert.Equal(200, res.statusCode);
        Assert.True(res.data);
        Assert.Empty(ctx.Contacts);
    }
}
