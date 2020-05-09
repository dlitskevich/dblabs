using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
	public class Customer {
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Purchase> Purchase { get; set; }

		public Customer() {
			Purchase = new List<Purchase>();
		}
	}
}
