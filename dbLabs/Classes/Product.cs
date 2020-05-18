using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbLabs.Classes {
	[Table("Products")]
	public class Product {
		public int Id { get; set; }

		[MaxLength(50)]
		[Column(TypeName = "varchar(50)")]
		public string Name { get; set; }
		public ICollection<ShopItem> ShopItems { get; set; }

		public Product() {
			ShopItems = new List<ShopItem>();
		}
	}
}
