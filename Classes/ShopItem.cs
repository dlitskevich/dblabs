using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dbLabs.Classes {
	[Table("ShopItems")]
	public class ShopItem {
		public int Id { get; set; }
		[Required]
		public int ProductId { get; set; }
		public int ProviderId { get; set; }

		[Required]
		public int Amount { get; set; }
		public int Price { get; set; }
		[Column(TypeName = "enum('Happy','Sad')")]
		public Happiness Happiness { get; set; }


		public virtual Product Product { get; set; }
		public Provider Provider { get; set; }
		public ICollection<Purchase> Purchase { get; set; }

		public ShopItem() {
			Purchase = new List<Purchase>();
		}

		//public bool Buy(int amount) {
		//	if(Amount - amount < 0 || amount < 1) {
		//		return false;
		//	}
		//	Amount -= amount;
		//	return true;
		//}

	}

	public enum Happiness {
		Happy = 1,
		Sad = 2
	}
}
