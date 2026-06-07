using Booking_Meeting_Rooms.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking_Meeting_Rooms.Context
{
    public class AddDbContext : DbContext
    {

        private readonly IConfiguration _configuration;

        public DbSet<User> Users { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Equipment> Equipment { get; set; }

        public DbSet<Bookings> Bookings { get; set; }

        public AddDbContext(DbContextOptions<AddDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Equipment>()
            .HasData(new Equipment { Id = 1, Name = "Projector" }, new Equipment { Id = 2, Name = "Board" }, new Equipment { Id = 3, Name = "WiFi"});

            var passwordHasher = new PasswordHasher<User>();


            var user = new User
            {
                Id = 1,
                Name = "Admin",
                Email = "Admin@gmail.com",
                Role = Enums.Role.Admin,
            };

            var AdminPassword = _configuration["AdminPassword:Password"] ?? "1234";

            user.Password = passwordHasher.HashPassword(user, AdminPassword);


            modelBuilder.Entity<User>()
                .HasData(user);


            modelBuilder.Entity<Room>()
                .HasMany(x => x.Equipment)
                .WithMany();

           
            


        }

    }
}
