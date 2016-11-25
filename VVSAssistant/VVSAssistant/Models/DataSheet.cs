using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VVSAssistant.Models
{
    public abstract class DataSheet
    {
        [Browsable(false)]
        public int Id { get; set; }

        [Category("Andet")]
        [DisplayName(@"Pris")]
        public double Price { get; set; }
    }
}
