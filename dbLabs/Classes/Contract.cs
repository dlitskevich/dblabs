using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbLabs.Classes {
	[Table("Contracts")]
	public class Contract {
		public int Id { get; set; }
		public string Info { get; set; }
		public DateTime Start { get; set; }
		public DateTime? End { get; set; }

		public Staff Staff { get; set; }

		public Contract() {
			Start = DateTime.Now;
			End = null;
		}
	}

	
}
