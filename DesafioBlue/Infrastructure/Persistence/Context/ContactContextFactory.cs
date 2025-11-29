using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DesafioBlue.Infrastructure.Persistence.Context
{
    public class ContactContextFactory : IDesignTimeDbContextFactory<ContactContext>
    {
        public ContactContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ContactContext>();

            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                                   ?? "Host=localhost;Database=desafioblue;Username=postgres;Password=postgres";

            builder.UseNpgsql(connectionString);

            return new ContactContext(builder.Options);
        }
    }
}
