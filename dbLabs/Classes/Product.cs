using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
	public class Product {
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<ShopItem> ShopItems { get; set; }

		public Product() {
			ShopItems = new List<ShopItem>();
		}
	}
}
