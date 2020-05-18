using System;
using System.Collections.Generic;

namespace dbLabsDummy.maskShop
{
    public partial class Customers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CustomerType { get; set; }
        public int? Discount { get; set; }
    }
}
