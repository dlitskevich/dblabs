using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
	public class ShopItem {
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int ProviderId { get; set; }

		public int Amount { get; set; }
		public int Price { get; set; }
		
		public virtual Product Product { get; set; }
		public Provider Provider { get; set; }
		public ICollection<Purchase> Purchase { get; set; }

		public ShopItem() {
			Purchase = new List<Purchase>();
		}

	}
}
