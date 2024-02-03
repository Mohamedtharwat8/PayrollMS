using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace PayrollMS.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employeer> Employees { get; set; }
        public DbSet<Salary> Salarys { get; set; }
        public DbSet<SalaryRequest> SalaryRequests { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
