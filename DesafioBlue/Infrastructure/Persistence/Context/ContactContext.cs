using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DesafioBlue.Infrastructure.Persistence.Context
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

    }
}