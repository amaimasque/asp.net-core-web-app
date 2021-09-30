using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CoreWebApp.Models;

namespace CoreWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CoreWebApp.Models.EmployeeModel> EmployeeModel { get; set; }
        public DbSet<CoreWebApp.Models.ProductModel> ProductModel { get; set; }
    }
}
