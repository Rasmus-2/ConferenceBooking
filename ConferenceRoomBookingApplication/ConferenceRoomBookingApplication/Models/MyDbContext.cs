using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication.Models
{
    internal class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;database=ConferenceBooking;Trusted_connection=True;Trustservercertificate=True;");
        }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Company> Companies { get; set; }
        public DbSet<Models.Country> Countries { get; set; }
        public DbSet<Models.City> Cities { get; set; }
        public DbSet<Models.Booking> Bookings { get; set; }
        public DbSet<Models.Room> Rooms { get; set; }
        public DbSet<Models.Facility> Facilities { get; set; }
    }
}
