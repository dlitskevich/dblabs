using System;
namespace dbLabs.Classes {
	public class Purchase {
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int ShopItemId { get; set; }
		public int? StaffId { get; set; }
		public int Amount { get; set; }

		public Customer Customer { get; set; }
		public ShopItem ShopItem { get; set; }
		public Staff Staff { get; set; }

		
	}
}
