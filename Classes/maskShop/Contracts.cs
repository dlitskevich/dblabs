using System;
using System.Collections.Generic;

namespace dbLabsDummy.maskShop
{
    public partial class Contracts
    {
        public int Id { get; set; }
        public string Info { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }
}
