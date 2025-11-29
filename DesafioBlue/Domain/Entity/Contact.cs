using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBlue.Domain.Entity
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }
        [Phone]
        public required string Phone { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
    }
}

