using Microsoft.EntityFrameworkCore;
using Planeventbackend.Data.Maps;
using Planeventbackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :
            base(options)
        {
        }
        public DbSet<UserModel> users { get; set; }

        public DbSet<EventModel> events { get; set; }

        public DbSet<UserEventModel> userEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new EventMap());
            modelBuilder.ApplyConfiguration(new UserEventMap());
        }
    }
}
