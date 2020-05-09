using System;
using System.Collections.Generic;

namespace dbLabsDummy.world
{
    public partial class Countrylanguage
    {
        public string CountryCode { get; set; }
        public string Language { get; set; }
        public string IsOfficial { get; set; }
        public float Percentage { get; set; }

        public virtual Country CountryCodeNavigation { get; set; }
    }
}
