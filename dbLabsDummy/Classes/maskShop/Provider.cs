using System;
using System.Collections.Generic;

namespace dbLabsDummy.maskShop
{
    public partial class Provider
    {
        public Provider()
        {
            ShopItems = new HashSet<ShopItems>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public virtual ICollection<ShopItems> ShopItems { get; set; }
    }
}
