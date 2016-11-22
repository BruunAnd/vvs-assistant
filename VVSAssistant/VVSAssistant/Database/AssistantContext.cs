using System.Data.Entity;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Database
{
    public class AssistantContext : DbContext
    {
        public AssistantContext() : base("AssistantDatabaseConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackagedSolution>().HasMany(s => s.Appliances).
                WithMany().
                Map(m =>
                {
                    m.ToTable("AppliancePackageAssosocation");
                    m.MapLeftKey("PackagedSolutionId");
                    m.MapRightKey("ApplianceId");
                });

            // Create a table for each datasheet type
            modelBuilder.Entity<ContainerDataSheet>().ToTable("ContainerDataSheets");
            modelBuilder.Entity<HeatingUnitDataSheet>().ToTable("HeatingUnitDataSheets");
            modelBuilder.Entity<SolarCollectorDataSheet>().ToTable("SolarCollectorDataSheets");
            modelBuilder.Entity<TemperatureControllerDataSheet>().ToTable("TemperatureControllerDataSheet");
            modelBuilder.Entity<WaterHeatingUnitDataSheet>().ToTable("WaterHeatingUnitDataSheets");

            // Create a table for each unitprice type
            modelBuilder.Entity<Salary>().ToTable("Salaries");
            modelBuilder.Entity<Material>().ToTable("Materials");
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientInformation> ClientInformation { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferInformation> OfferInformation { get; set; }
        public DbSet<PackagedSolution> PackagedSolutions { get; set; }
        public DbSet<Appliance> Appliances { get; set; }
        public DbSet<UnitPrice> UnitPrices { get; set; }
        public DbSet<DataSheet> DataSheets { get; set; }
    }
}
