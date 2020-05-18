using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbLabs.Classes {
	[Table("Staffs")]
	public class Staff {
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual Contract Contract { get; set; }
		public ICollection<Purchase> Purchase { get; set; }

		public Staff() {
			Purchase = new List<Purchase>();
		}
	}
}
