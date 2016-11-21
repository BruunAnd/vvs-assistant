using System.Data.Entity;
using VVSAssistant.Models;

namespace VVSAssistant.Database
{
    public class AssistantContext : DbContext
    {
        public AssistantContext() : base("AssistantDatabaseConnectionString")
        {
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
