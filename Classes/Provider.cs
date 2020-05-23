using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbLabs.Classes {
	[Table("Providers")]
	public class Provider {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Info { get; set; }
		public ICollection<ShopItem> ShopItems { get; set; }

		public Provider() {
			ShopItems = new List<ShopItem>();
		}
	}
}
