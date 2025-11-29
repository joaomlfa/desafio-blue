using System.Collections.Generic;
using DesafioBlue.Infrastructure.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;
using Xunit;

namespace DesafioBlue.Tests.Tests;

public class BaseRepositoryTests
{
    private class TestRepo : BaseRepository
    {
        public TestRepo() : base() { }
        public int ExposedGetPageSize(int pageSize) => GetPageSize(pageSize);
        public ListPaginated<TAny> ExposedGetPaginatedResult<TAny>(List<TAny> items, int pageSize) where TAny : class
            => GetPaginatedResult(items, pageSize);
    }

    [Fact]
    public void GetPageSize_Default_When_NegativeOrZero()
    {
        var repo = new TestRepo();
        var size = repo.ExposedGetPageSize(0);
        Assert.Equal(51, size); // Default 50 + 1
    }

    [Fact]
    public void GetPageSize_Max_When_TooLarge()
    {
        var repo = new TestRepo();
        var size = repo.ExposedGetPageSize(1000);
        Assert.Equal(101, size); // Max 100 + 1
    }

    [Fact]
    public void GetPaginatedResult_HasNext_When_MoreItems()
    {
        var repo = new TestRepo();
        var items = new List<string> { "1", "2", "3" };
        var result = repo.ExposedGetPaginatedResult(items, 2);
        Assert.True(result.HasNext);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public void GetPaginatedResult_NoNext_When_FewerOrEqual()
    {
        var repo = new TestRepo();
        var items = new List<string> { "1", "2" };
        var result = repo.ExposedGetPaginatedResult(items, 2);
        Assert.False(result.HasNext);
        Assert.Equal(2, result.Items.Count);
    }
}
