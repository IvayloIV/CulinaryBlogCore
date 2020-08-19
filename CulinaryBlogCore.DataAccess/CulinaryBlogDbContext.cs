﻿using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlogCore.DataAccess
{
    public class CulinaryBlogDbContext : IdentityDbContext<ApplicationUser>
    {
        public CulinaryBlogDbContext(DbContextOptions<CulinaryBlogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
