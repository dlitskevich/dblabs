﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbLabs.Classes {
	[Table("Purchases")]
	public class Purchase {
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int? ShopItemId { get; set; }
		public int? StaffId { get; set; }
		public int Amount { get; set; }
		public DateTime Date { get; set; }

		public Customer Customer { get; set; }
		public ShopItem ShopItem { get; set; }
		public Staff Staff { get; set; }

		public Purchase() {
			Date = DateTime.Now;
		}
	}
	
}
