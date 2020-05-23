using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbLabs.Classes {
	[Table("Customers")]
	public class Customer {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Customer_Type { get; set; }
		public int? Discount { get; set; }
		public ICollection<Purchase> Purchase { get; set; }

		public Customer() {
			Purchase = new List<Purchase>();
		}
	}
}
