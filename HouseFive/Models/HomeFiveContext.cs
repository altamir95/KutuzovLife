using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Models
{
    public class HomeFiveContext : DbContext
    {
        public DbSet<HouseAdviser> HouseAdvisers { get; set; }
        public DbSet<MeetingMinutes> MeetingMinutes { get; set; }
        public DbSet<Initiative> Initiatives { get; set; }
        public DbSet<KeyValue> KeyValues { get; set; }
        public DbSet<Appeal> Appeals { get; set; }
        public DbSet<InitiativeAppeal> InitiativeAppeal { get; set; }
        public HomeFiveContext(DbContextOptions<HomeFiveContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InitiativeAppeal>()
                .HasKey(c => new { c.InitiativeId, c.AppealId });
        }
    }
}
