using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OrganizationsWaterSupplyL4.Models;

namespace OrganizationsWaterSupplyL4.Data
{
    public partial class OrganizationsWaterSupplyContext : DbContext
    {
        public OrganizationsWaterSupplyContext()
        {
        }

        public OrganizationsWaterSupplyContext(DbContextOptions<OrganizationsWaterSupplyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Counter> Counters { get; set; } = null!;
        public virtual DbSet<CounterModel> CounterModels { get; set; } = null!;
        public virtual DbSet<CountersCheck> CountersChecks { get; set; } = null!;
        public virtual DbSet<CountersDatum> CountersData { get; set; } = null!;
        public virtual DbSet<Organization> Organizations { get; set; } = null!;
        public virtual DbSet<Rate> Rates { get; set; } = null!;
        public virtual DbSet<RateOrgNote> RateOrgNotes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("SqlServerConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
