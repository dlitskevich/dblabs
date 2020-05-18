using System;
using System.Collections.Generic;

namespace dbLabsDummy.maskShop
{
    public partial class ShopItems
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProviderId { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public string Happiness { get; set; }

        public virtual Provider Provider { get; set; }
    }
}
