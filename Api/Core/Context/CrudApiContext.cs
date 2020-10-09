using Core.Data.Configurations;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Context
{
    public sealed class CrudApiContext : DbContext
    {
        public CrudApiContext(DbContextOptions<CrudApiContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Food> Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new FoodConfiguration());
        }
    }
}