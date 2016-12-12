using System.Data.Entity;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Database
{
    public class AssistantContext : DbContext
    {
        public AssistantContext() : base("AssistantDatabaseConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Map PackagedSolution
            modelBuilder.Entity<PackagedSolution>().HasMany(s => s.ApplianceInstances).WithRequired();

            //// Map Appliance
            modelBuilder.Entity<Appliance>().HasRequired(a => a.DataSheet);

            // Map Client
            modelBuilder.Entity<Client>().HasRequired(c => c.ClientInformation);

            // Create a table for each datasheet type
            modelBuilder.Entity<ContainerDataSheet>().ToTable("ContainerDataSheets");
            modelBuilder.Entity<HeatingUnitDataSheet>().ToTable("HeatingUnitDataSheets");
            modelBuilder.Entity<SolarCollectorDataSheet>().ToTable("SolarCollectorDataSheets");
            modelBuilder.Entity<SolarStationDataSheet>().ToTable("SolarStationDataSheets");
            modelBuilder.Entity<TemperatureControllerDataSheet>().ToTable("TemperatureControllerDataSheets");

            //// Map ApplianceInstance
            modelBuilder.Entity<ApplianceInstance>().HasRequired(a => a.Appliance).WithMany();

            // Map Offer
            modelBuilder.Entity<Offer>().HasMany(o => o.Materials);
            modelBuilder.Entity<Offer>().HasMany(o => o.Salaries);
            modelBuilder.Entity<Offer>().HasMany(o => o.Appliances);
            modelBuilder.Entity<Offer>().HasRequired(o => o.PackagedSolution);
        }

        public DbSet<CompanyInformation> CompanyInformation { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientInformation> ClientInformation { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferInformation> OfferInformation { get; set; }
        public DbSet<PackagedSolution> PackagedSolutions { get; set; }
        public DbSet<Appliance> Appliances { get; set; }
        public DbSet<UnitPrice> UnitPrices { get; set; }
        public DbSet<DataSheet> DataSheets { get; set; }
        public DbSet<ApplianceInstance> ApplianceInstances { get; set; }
    }
}
