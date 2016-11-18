//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class Client
    {
        /* Lazy loading has been removed - should it be used? */
        public Client()
        {
            Offers = new List<Offer>();
        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public ClientInformation ClientInformation { get; set; }
        public ICollection<Offer> Offers { get; set; }
    }
}
