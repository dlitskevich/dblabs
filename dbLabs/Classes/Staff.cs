using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
	public class Staff {
		public int Id { get; set; }
		public string Name { get; set; }
		public Contract Contract { get; set; }
		public ICollection<Purchase> Purchase { get; set; }

		public Staff() {
			Purchase = new List<Purchase>();
		}
	}
}
