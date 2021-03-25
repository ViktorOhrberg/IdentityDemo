using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Models
{
    public class MyIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class MyIdentityDbContext : IdentityDbContext<MyIdentityUser>
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
        : base(options)
        {
            // Create DB schema (only works against an empty database)
            var result = Database.EnsureCreated();
        }
    }
}
