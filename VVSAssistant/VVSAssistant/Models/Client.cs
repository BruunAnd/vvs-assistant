//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class Client
    {
        public Client()
        {
            Offers = new List<Offer>();
            ClientInformation = new ClientInformation();
        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ClientInformation ClientInformation { get; set; }
        public virtual ICollection<Offer> Offers { get; }
    }
}
