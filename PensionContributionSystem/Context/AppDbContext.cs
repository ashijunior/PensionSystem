

using Microsoft.EntityFrameworkCore;
using PensionContributionSystem.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PensionContributionSystem.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Employer> Employers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
