namespace Core.Context
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    internal sealed class DesignTimeContextFactory : IDesignTimeDbContextFactory<CrudApiContext>
    {
        CrudApiContext IDesignTimeDbContextFactory<CrudApiContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CrudApiContext>();
            optionsBuilder.UseSqlServer("Server=ALEXCIURARIU2\\SQLEXPRESS; Database=CrudApi;Trusted_Connection=True;MultipleActiveResultSets=True");

            return new CrudApiContext(optionsBuilder.Options);
        }
    }
}