using System;
using System.Collections.Generic;

namespace dbLabsDummy.maskShop
{
    public partial class Purchases
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? ShopItemId { get; set; }
        public int? StaffId { get; set; }
        public int Amount { get; set; }
        public DateTime Time { get; set; }
    }
}
