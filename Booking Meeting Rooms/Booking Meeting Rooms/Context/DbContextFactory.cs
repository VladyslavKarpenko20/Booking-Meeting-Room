using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Booking_Meeting_Rooms.Context
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AddDbContext>
    {

        public AddDbContext CreateDbContext(string[] args)
        {

            var basePath = AppContext.BaseDirectory;

            var configurations = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AddDbContext>();

            optionsBuilder.UseNpgsql(configurations["Database:Strings"]);


            return new AddDbContext(optionsBuilder.Options, configurations);
        }

    }
}
