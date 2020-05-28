using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Configuration;
using System.Diagnostics;
using Syncfusion.Data.Extensions;
using Syncfusion;
using AppKit;
using dbLabs.Classes;
using dbLabs.world;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Syncfusion.SfDataGrid.XForms;
using Microsoft.EntityFrameworkCore.ChangeTracking;

///
/// stored procedures
///https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
/// TODO:
/// 
///

namespace dbLabs {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage {

		//private string connString = "server=127.0.0.1; user=root; password=Password; database=autoschool";
		public class change { 
			public DateTime time;
			public PropertyValues value;
			public EntityState state;
		}
	
	
		private ShopContext context;
		private worldContext worldContext;
		private Dictionary<EntityEntry, change> changedStack = new Dictionary<EntityEntry, change>();

		public MainPage() {

			using(ShopContext db = new ShopContext()) {

				//db.Database.Migrate();
				
			}

			InitializeComponent();


		}

		public void AddToStack(object sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e) {
			//var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
			var changedEntries = context.ChangeTracker.Entries();
			var x = context.ChangeTracker.Entries();
			//e.Entry.CurrentValues;
			//changedStack.Add(q, new change() { time = DateTime.Now, value = q.CurrentValues.Clone() });
			var y = e.Entry;
			changedStack.Add(y, new change() {
				time = DateTime.Now,
				state = y.State,
				value = y.CurrentValues.Clone()
			});
		}
	

		public void AddToStackTrack(object sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityTrackedEventArgs e) {
			//var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
			//var changedEntries = context.ChangeTracker.Entries();
			var x = context.ChangeTracker.Entries();

		}

		public void AddToStackEdit(object sender, GridCurrentCellEndEditEventArgs e) {
			var changedEntries = context.ChangeTracker.Entries().ToList();
			//var changedEntries = context.ChangeTracker.Entries();
			//var rowID = e.RowColumnIndex.RowIndex;
			var x = ShowGrid.SelectedItem;
			var y = changedEntries.Find(z => z.Entity == x);
			if(y != null) {
				changedStack.Add(y, new change() {
					time = DateTime.Now,
					state = y.State,
					value = y.CurrentValues.Clone() });
			}

		}


		private void CancelLast(object sender, EventArgs e) {
			if(changedStack.Count > 0) {
				var row = changedStack.Aggregate((x, y) => x.Value.time > y.Value.time ? x : y);
				
				row.Key.CurrentValues.SetValues(row.Value.value);
				row.Key.State = row.Value.state;
						
				changedStack.Remove(row.Key);
			}
			ShowContracts(null, null);
			ShowShopItems(null, null);
		}

		private void ShowLastChoice(object sender, EventArgs e) {
			var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
			var x = ShowGrid.SelectedItem;
			var y = changedEntries.Find(z => z.Entity == x);

			if(changedStack.Count > 0 && x!=null) {
				var ee = changedStack.Where(el => el.Key.Entity == y.Entity).ToDictionary(i => i.Key, i => i.Value);
				//var e1e = changedStack.Where(el => el.Key.Entity == y.Entity).Select(el => el.Value.value.GetValue<int>("Amount"));
				var e1e = changedStack.Where(el => el.Key.Entity == y.Entity).Select(el => el.Value.value.ToObject());
				resultGrid.ItemsSource = e1e.ToList();

			}
			ShowGrid.Refresh();
		}

		private void CancelLastChoice(object sender, EventArgs e) {
			var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
			var x = ShowGrid.SelectedItem;
			var y = changedEntries.Find(z => z.Entity == x);

			if(changedStack.Count > 0 && x != null) {
				var ee = changedStack.Where(el => el.Key.Entity == y.Entity).ToDictionary(i => i.Key, i => i.Value);
				var e1e = changedStack.Where(el => el.Key.Entity == y.Entity).Select(el => el.Value.value);
				

				var z = resultGrid.SelectedIndex;
				if(z != -1) {
					y.CurrentValues.SetValues(ee.ElementAt(z-1).Value.value);
					y.State = ee.ElementAt(z-1).Value.state;
				}
			}
			ShowGrid.Refresh();
		}

		public void EndEditing(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			var x = context.ChangeTracker.Entries();
		}



		public void PageLoaded(object sender, EventArgs e) {
			context = new ShopContext();
			worldContext = new worldContext();

			context.ChangeTracker.StateChanged += AddToStack;
			//Microsoft.EntityFrameworkCore;
			context.ChangeTracker.Tracked += AddToStackTrack;

			var x = context.ChangeTracker.Entries();

			context.ShopItems.Load();
			ShowGrid.ItemsSource = context.ShopItems.Local.ToBindingList();

			ShowGrid.CurrentCellEndEdit += AddToStackEdit;

			/*

			ShowGrid.AutoGeneratingColumn += datagrid_AutoGeneratingColumn;

			void datagrid_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e) {
				if(e.Column.MappingName == "EmployeeDate") {
					// Setting default date and time format for EmployeeDate column
					((e.Column) as GridDateTimeColumn).Pattern = GridDateTimeColumn.P;
						//Syncfusion.SfDataGrid.XForms.Da.
						//SfDataGrid.XForms.SfDataGrid.Auto.Shared.DateTimePattern.FullDateTime;
				}
			}
			*/
			/*
			worldContext.Country.Load();
			var result = from country in worldContext.Country.ToList()
						 let maxArea = (
							from countr in worldContext.Country.ToList()
							group countr by new {
								id = countr.Continent
							} into countryGrouped
							where countryGrouped.Key.id.ToLower() == "africa"
							select new { max = countryGrouped.Max(x => x.SurfaceArea) }

							).FirstOrDefault().max
						 where country.SurfaceArea > maxArea
						 select new {
							 Name = country.Name,
							 Area = country.SurfaceArea,
							 AfricaMaxArea = maxArea
						 };
			resultGrid.ItemsSource = result;

			CheckFK(null, null);
			*/
			//

			/////////////////////
			// Greedy (not virtual props)   .Include
			/////////////////////
			/*
			List<Candidate> curCand = context.Candidates.Include(p => p.Confidents).ToList<Candidate>();
			string result = "";
			foreach(var c in curCand) {
				result += c.Name + " " + c.Surname + " \n" + "Confidents: ";
				foreach(var conf in c.Confidents) {
					result += conf.FullName + " ";
				}
				result += "\n";
			System.Console.WriteLine(result);
			}
			*/
			/////////////////////
			// Lazy (set virtual props) delayed   Directly
			/////////////////////
			/*
			var curCand1 = context.Candidates.ToList();
			var result = "";
			foreach(var c in curCand1) {
				result += c.Name + " " + c.Surname + " \n" + "Confidents: ";
				foreach(var conf in c.Confidents) {
					result += conf.FullName + " ";
				}
				result += "\n";
			}
			*/
			/////////////////////
			// Explicit (not virtual props) delayed     .Load()
			/////////////////////
			/*
			var curCand = context.Candidates.FirstOrDefault();
			context.Entry(curCand).Collection("Confidents").Load();
			string result = "";
			result += curCand.Name + " " + curCand.Surname + " \n" + "Confidents: ";
			foreach(var conf in curCand.Confidents) {
				result += conf.FullName + " ";
			}
			*/

		}
		public void PageClosed(object sender, EventArgs e) {
			context.Dispose();
		}

		private void Sync(object sender, EventArgs e) {
			context.SaveChanges();
		}

		private void ShowContracts(object sender, EventArgs e) {
			context.Contracts.Load();
			ShowGrid.ItemsSource = context.Contracts.Local.ToList();
		}

		private void ShowCustomers(object sender, EventArgs e) {
			context.Customers.Load();
			ShowGrid.ItemsSource = context.Customers.Local.ToBindingList();
		}
		//private void ShowVIPs(object sender, EventArgs e) {
		//	context.VIPs.Load();
		//	ShowGrid.ItemsSource = context.VIPs.Local.ToList();
		//}

		private void ShowShopItems(object sender, EventArgs e) {
			context.ShopItems.Load();
			ShowGrid.ItemsSource = context.ShopItems.Local.OrderBy(i => i.Id);
		}

		private void ShowPurchase(object sender, EventArgs e) {
			context.Purchases.Load();
			ShowGrid.ItemsSource = context.Purchases.Local.OrderBy(p => p.Id);

		}

		private void ShowProducts(object sender, EventArgs e) {
			context.Products.Load();
			ShowGrid.ItemsSource = context.Products.Local.OrderBy(p => p.Id);

		}

		private void UpdateShopItem(object sender, EventArgs e) {
			try {
				var toUpdShop = (from sh in context.ShopItems.AsEnumerable()
								 where sh.Amount > 10000
								 select sh).ToList();
				foreach(ShopItem item in toUpdShop) {
					item.Price = 1;
				}

				//context.SaveChanges();
			} catch(Exception ex) {
				System.Console.WriteLine(ex.Message);
			}
			ShowGrid.Refresh();
		}

		private void AddShopItem(object sender, EventArgs e) {

			context.ShopItems.Add(new ShopItem { Amount = 0, Price = 0, ProductId = 1, ProviderId = 1 });
			context.SaveChanges();
		}
		/// /////////////// ////////////
		/// /////////////// ////////////
		// simpe test (if set fkID its automaticaly ads its object)
		/// /////////////// ////////////
		private void CheckFK(object sender, EventArgs e) {
			var testFK = context.ShopItems.Include(s => s.Product).ToList<ShopItem>();
			Console.WriteLine();
		}



		private void RemoveShopItem(object sender, EventArgs e) {
			List<ShopItem> shop = context.ShopItems.Include(s => s.Purchase).ToList<ShopItem>();
			try {
				var toDelShop = (from sh in shop
								 where sh.Amount < 1000
								 select sh).ToList();
				for(int i = 0; i < toDelShop.Count(); i++) {
					ShopItem curItem = toDelShop[i] as ShopItem;
					if(curItem != null) {
						
						foreach(Purchase purch in curItem.Purchase) {
							context.Purchases.Remove(purch);
						}
						context.ShopItems.Remove(curItem);
					}
				}
				ShowPurchase(null, null);
				ShowShopItems(null, null);

				//context.SaveChanges();
			} catch(Exception ex) {
				System.Console.WriteLine(ex.Message);
			}

		}


		private void RemoveContract(object sender, EventArgs e) {
			try {
				if(ShowGrid.SelectedItems != null) {
					for(int i = 0; i < ShowGrid.SelectedItems.Count; i++) {
						Contract curCand = ShowGrid.SelectedItems[i] as Contract;
						if(curCand != null) {
							context.Contracts.Remove(curCand);
						}
					}
				}
				context.SaveChanges();
			} catch(Exception ex) {
				System.Console.WriteLine(ex.Message);
			}
		}

		/// ///////////////// ///////////////// ///////////////// //////////////
		/// /// ///////////////// //////////////
		/// /// ///////////////// //////////////
		/// /// ///////////////// //////////////

		private void BuyItem(object sender, EventArgs e) {
			//try {
			//	if(ShowGrid.SelectedItem != null) {
			//		ShopItem item = ShowGrid.SelectedItem as ShopItem;
			//		var amount = int.TryParse(buyAmount.Text, out int min) ? min : 0;
			//		var res = context.Database.ExecuteSqlRaw($"call make_purchase({item.Id}, {amount})");

			//		//context.MakePurchase(item, int.TryParse(buyAmount.Text, out int min) ? min : 0);

			//	}
			//	context.SaveChanges();
			//} catch(Exception ex) {
			//	System.Console.WriteLine(ex.Message);
			//}

			using(var transaction = context.Database.BeginTransaction()) {
				try {
					if(ShowGrid.SelectedItem != null) {
						ShopItem item = ShowGrid.SelectedItem as ShopItem;
						var amount = int.TryParse(buyAmount.Text, out int min) ? min : 0;
						var res = context.Database.ExecuteSqlRaw($"call make_purchase({item.Id}, {amount})");

						context.SaveChanges();
					}
					transaction.Commit();

				} catch(Exception ex) {
					System.Console.WriteLine(ex.Message);
					transaction.Rollback();
				}
			}
		}

		//private void Rating_Click(object sender, EventArgs e) {
		//	Candidate firstCandidate = context.Candidates.FirstOrDefault();
		//	System.Random rnd = new System.Random();
		//	int myInt = rnd.Next(1, 10000);
		//	float myFloat = myInt / 100;
		//	firstCandidate.RatingChange(myFloat);

		//	Candidate secondCandidate = context.Candidates.OrderBy(x => x.Id).Skip(1).FirstOrDefault();
		//	secondCandidate.RatingChange(100 - myFloat); context.SaveChanges();
		//	ShowGrid.Refresh();
		//}

		//private void Edit_Surname(object sender, EventArgs e) {
		//	if(ShowGrid.SelectedItem != null) {
		//		Candidate curCand = ShowGrid.SelectedItem as Candidate;
		//		curCand.Surname = editSurname.Text;
		//		context.SaveChanges();
		//		ShowGrid.Refresh();
		//	}
		//}

		//private void query_Click(object sender, EventArgs e) {
		//	using(ElectionContext db = new ElectionContext()) {
		//		var result = db.Candidates.Where(x => x.Rating > 10).ToList();

		//		var result2 = db.Candidates.Join(db.Confidents,
		//			  p => p.Id,
		//			  c => c.CandidateId,
		//			  (p, c) => new {
		//				  Name = c.FullName,
		//				  CandidateSurname = p.Surname
		//			  }).ToList();

		//		var result3 = db.Confidents.AsEnumerable()
		//			.GroupBy(c => c.CandidateId)
		//			.Select(g => new { ID = g.FirstOrDefault().Id, AvgAge = g.Average(c => c.Age) })
		//			.Join(db.Candidates.AsEnumerable(),
		//			a => a.ID,
		//			b => b.Id,
		//			(a, b) => new {
		//				Name = b.Name,
		//				Surname = b.Surname,
		//				Age = a.AvgAge
		//			}
		//			).ToList();
		//		resultGrid.ItemsSource = result2;
		//	}
		//}

		private void ProductAVGamount(object sender, EventArgs e) {
			var purchases = context.Purchases.Include(p => p.ShopItem).Include(si => si.ShopItem.Product).ToList();
			var result = from purch in purchases
						 group purch by purch.ShopItem.Product.Name into amount
						 select new { Name = amount.Key, AVGamount = amount.Average(a => a.Amount) }
						;
			resultGrid.ItemsSource = result;
		}

		private void CustomerTotal(object sender, EventArgs e) {
			var purchases = context.Purchases.Include(p => p.Customer).Include(p => p.ShopItem).ToList();
			var result = purchases
				.GroupBy(p => p.Customer.Id)
				.Select(p => new {
					Name = p.FirstOrDefault().Customer.Name,
					Total = p.Sum(x => x.Amount * x.ShopItem.Price)
				});
			;
			resultGrid.ItemsSource = result;
		}

		private void ProviderInfo(object sender, EventArgs e) {
			var items = context.ShopItems.ToList();
			var provs = context.Providers.ToList();
			var result = from item in items
						 join prov in provs
						 on item.ProviderId equals prov.Id
						 group new { item, prov }
						 by new {
							 prov.Id,
							 prov.Name,
							 prov.Info
						 } into itemProv
						 orderby itemProv.Key.Id ascending
						 select new {
							 itemProv.Key.Id,
							 itemProv.Key.Name,
							 itemProv.Key.Info,
							 AVGprice = itemProv.Average(x => x.item.Price),
							 ManufQuantity = itemProv.Sum(x => x.item.Amount)
						 };

			resultGrid.ItemsSource = result;
		}

		private void PurchaseInfo(object sender, EventArgs e) {
			var purchs = context.Purchases.ToList();
			var items = context.ShopItems.ToList();
			var provs = context.Providers.ToList();
			var result = from purch in purchs

						 join cust in context.Customers.ToList()
						 on purch.CustomerId equals cust.Id

						 join staff in context.Staffs.ToList()
						 on purch.StaffId equals staff.Id

						 join item in (from item in items
									   join prov in provs
									   on item.ProviderId equals prov.Id
									   join prod in context.Products.ToList()
									   on item.ProductId equals prod.Id
									   select new {
										   item.Id,
										   prod.Name,
										   Provider = prov.Name,
										   item.Price
									   }
										   )
						 on purch.ShopItemId equals item.Id

						 orderby purch.Id ascending
						 select new {
							 purch.Id,
							 Customer = cust.Name,
							 Product = item.Name,
							 Seller = staff.Name,
							 item.Provider,
							 purch.Amount,
							 item.Price
						 };

			resultGrid.ItemsSource = result;
		}



		/// Cancelations
		/// !!!!!!!!!!!!
		///

		private void Cancel(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

			foreach(var entry in changedEntries) {
				switch(entry.State) {
				case EntityState.Modified: {
						entry.CurrentValues.SetValues(entry.OriginalValues);
						entry.State = EntityState.Unchanged;
						ShowGrid.Refresh();
						break;
					}
				case EntityState.Deleted: {
						entry.State = EntityState.Unchanged;
						ShowGrid.Refresh();
						break;
					}
				case EntityState.Added: {
						entry.State = EntityState.Detached;
						ShowGrid.Refresh();
						break;
					}

				}
			}
		}

		private void CancelDeleted(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

			foreach(var entry in changedEntries) {

				if(entry.State == EntityState.Deleted) {
					entry.State = EntityState.Unchanged;
					ShowGrid.Refresh();
				}
			}
		}
	

		private void CancelModified(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

			foreach(var entry in changedEntries) {

				if(entry.State == EntityState.Modified) {
					entry.CurrentValues.SetValues(entry.OriginalValues);
					entry.State = EntityState.Unchanged;
					ShowGrid.Refresh();
				}

			}
		}

	}
}
