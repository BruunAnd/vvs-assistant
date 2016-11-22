using System.Data.Entity;
using VVSAssistant.Models;

namespace VVSAssistant.Database
{
    public class AssistantContext : DbContext
    {
        public AssistantContext() : base("AssistantDatabaseConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackagedSolution>().HasMany(s => s.Appliances).WithMany(a => a.PackagedSolutions);
            modelBuilder.Entity<Appliance>().HasRequired(a => a.DataSheet);
            modelBuilder.Entity<Client>().HasRequired(c => c.ClientInformation);
            modelBuilder.Entity<Client>().HasMany(c => c.Offers).WithRequired(o => o.Client);
            modelBuilder.Entity<Offer>().HasRequired(o => o.OfferInformation);
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
