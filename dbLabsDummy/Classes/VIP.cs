using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
	public class VIP : Customer {
		public int Discount;

		public VIP() {
			Purchase = new List<Purchase>();

		}
	}
}

