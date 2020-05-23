using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbLabs.Classes {
	[Table("Test")]
	public class Test {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Test_Type { get; set; }
		//public int? Discount { get; set; }
		public ICollection<Purchase> Purchase { get; set; }

		public Test() {
			Purchase = new List<Purchase>();
		}
	}
}
