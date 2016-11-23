using System;
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

        public void Seed()
        {
            var loganoPlusAppliance = new Appliance
            {
                Name = "Logano Plus",
                CreationDate = DateTime.Now,
                Type = ApplianceTypes.Boiler,
                DataSheet = new HeatingUnitDataSheet {AFUE = 91, WattUsage = 18}
            };

            var loganoSknAppliance = new Appliance
            {
                Name = "Logasol SKN",
                CreationDate = DateTime.Now,
                Type = ApplianceTypes.SolarPanel,
                DataSheet = new SolarCollectorDataSheet {Area = 2.25f, Efficency = 60}
            };

            var bstAppliance = new Appliance
            {
                Name = "BST",
                CreationDate = DateTime.Now,
                Type = ApplianceTypes.Container,
                DataSheet = new ContainerDataSheet {Volume = 481, Classification = "C"}
            };

            // Create example packaged solution
            var packagedSolution = new PackagedSolution()
            {
                Name = "Example Solution",
                CreationDate = DateTime.Now
            };
            packagedSolution.Appliances.Add(bstAppliance);
            packagedSolution.Appliances.Add(loganoPlusAppliance);
            packagedSolution.Appliances.Add(loganoSknAppliance);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackagedSolution>().HasMany(s => s.ApplianceInstances).WithRequired();
            modelBuilder.Entity<PackagedSolution>().HasOptional(s => s.SolarContainerInstance);
            modelBuilder.Entity<PackagedSolution>().HasOptional(s => s.PrimaryHeatingUnitInstance);

            modelBuilder.Entity<Appliance>().HasRequired(a => a.DataSheet);

            modelBuilder.Entity<Client>().HasRequired(c => c.ClientInformation);

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
        public DbSet<ApplianceInstance> ApplianceInstances { get; set; }
    }
}
