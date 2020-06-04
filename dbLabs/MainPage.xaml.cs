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
using System.Linq;
using Syncfusion.SfDataGrid.XForms;

using MongoDB.Bson;
using MongoDB.Driver;

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

		public static string conn = "mongodb://127.0.0.1:27017";
		public static MongoClient clientMDB = new MongoClient(conn);
		public static IMongoDatabase database = clientMDB.GetDatabase("maskshop");
		public static IMongoCollection<Customer> customer = database.GetCollection<Customer>("customer");
		public static IMongoCollection<Provider> provider = database.GetCollection<Provider>("provider");
		public static IMongoCollection<Item> item = database.GetCollection<Item>("item");
		public static IMongoCollection<Purchase> purchase = database.GetCollection<Purchase>("purchase");
		public static IMongoCollection<Staff> staff = database.GetCollection<Staff>("staff");

		public MainPage() {

			

			InitializeComponent();


		}


		public void EndEditing(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			
		}



		public void PageLoaded(object sender, EventArgs e) {
			
			ShowGrid.ItemsSource = customer;

			

			//datagrid mongo

		}/*

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
		/*
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
		*/
		/// 
		/// ////////////////
		/// d
		/// 
		public static void Find<T>(IMongoCollection<T> collection, BsonDocument filter) {
			using(var cursor = collection.FindSync(filter)) {
				while(cursor.MoveNext()) {

					var entry = cursor.Current;
					foreach(var doc in entry) {
						foreach(var p in doc.ToBsonDocument().Elements) {
							Console.WriteLine(p);
						}
						Console.WriteLine();
					}
				}
			}
		}


		public class Purchase {
			public ObjectId Id { get; set; }
			public BsonDocument Customer { get; set; }
			public BsonDocument Staff { get; set; }
			public BsonArray Items { get; set; }
			public string Time { get; set; }

			public Purchase Create() {
				bool enough = false;
				Items = new BsonArray();

				Customer customer_current = new Customer().Find();
				Staff staff_current = new Staff().Find();

				while(!enough) {
					Console.WriteLine("  Please enter item info  ");
					Item new_item = new Item().Find();
					Console.WriteLine("  Please enter amount ");
					var amount = Console.ReadLine();

					Items.Add(new BsonDocument { { "Name", new_item.Name }, { "Amount", amount }, { "Name", new_item.Price }, { "Name", new_item.Id } });

					Console.WriteLine("  To stop enter anything  ");
					var next = Console.ReadLine();
					if(next != null) {
						enough = true;
					}
				}


				this.Customer = new BsonDocument { { "Name", customer_current.Name }, { "_id", customer_current.Id } };
				this.Staff = new BsonDocument { { "Name", staff_current.Name }, { "_id", staff_current.Id } };

				Time = DateTime.Now.ToShortDateString();

				return this;
			}

			public void MakePurchase() {
				bool enough = false;
				Items = new BsonArray();

				Customer customer_current = new Customer().Find();
				Staff staff_current = new Staff().Find();

				while(!enough) {
					double a;
					Console.WriteLine("  Please enter item info  ");
					Item new_item = new Item().Find();
					Console.WriteLine("  Please enter amount ");
					var amount = double.TryParse(Console.ReadLine(), out a) ? a : default(double);

					if(new_item.Amount > amount && amount > 0) {
						Items.Add(new BsonDocument { { "Name", new_item.Name }, { "Amount", amount }, { "Price", new_item.Price }, { "id", new_item.Id } });
						item.FindOneAndUpdate<Item>(Builders<Item>.Filter.Eq("_id", new_item.Id),
								Builders<Item>.Update.Set("Amount", new_item.Amount - amount)
							);
					} else {
						Console.WriteLine("  Not added ");
					}

					if(Items.Count > 0) {
						Console.WriteLine("  To stop enter anything  ");
						var next = Console.ReadLine();
						if(next != null) {
							enough = true;
						}
					}

				}


				this.Customer = new BsonDocument { { "Name", customer_current.Name }, { "_id", customer_current.Id } };
				this.Staff = new BsonDocument { { "Name", staff_current.Name }, { "_id", staff_current.Id } };

				Time = DateTime.Now.ToShortDateString();

				purchase.InsertOne(this);

				return;
			}

		}


		public class Item {
			public ObjectId Id { get; set; }
			public string Name { get; set; }
			public BsonDocument Provider { get; set; }
			public double Amount { get; set; }
			public double Price { get; set; }

			public Item Create() {
				double a;
				Console.WriteLine("Please enter item Name : ");
				Name = Console.ReadLine();

				Provider prov = new Provider().Find();

				this.Provider = new BsonDocument { { "_id", prov.Id }, { "Name", prov.Name } };

				Console.WriteLine("Please enter item Amount : ");
				Amount = double.TryParse(Console.ReadLine(), out a) ? a : default(double);
				Console.WriteLine("Please enter item Price : ");
				Price = double.TryParse(Console.ReadLine(), out a) ? a : default(double);

				return this;
			}

			public Item Find() {
				Console.WriteLine("Please enter item Name : ");
				Name = Console.ReadLine();

				Provider prov = new Provider().Find();

				var found = item.FindSync<Item>(new BsonDocument("$and",
					new BsonArray {
						new BsonDocument( "Name", new BsonDocument("$regex", Name) ),
						new BsonDocument("Provider.Name", new BsonDocument("$regex", prov.Name) )
					})).FirstOrDefault();

				if(found == null) {
					Console.WriteLine("Nothing found. Try again : ");
					return this.Find();
				}

				return found;
			}
		}

		public class Provider {
			public ObjectId Id { get; set; }
			public string Name { get; set; }
			public string Info { get; set; }

			public Provider Find() {
				Console.WriteLine("Please enter provider Name : ");
				Name = Console.ReadLine();

				var found = provider.FindSync<Provider>(new BsonDocument("Name", new BsonDocument("$regex", Name))).FirstOrDefault();

				if(found == null) {
					Console.WriteLine("Nothing found. Try again : ");
					return this.Find();
				}
				//Id = (ObjectId)found.GetValue("_id");
				//Name = (string)found.GetValue("Name");
				//Info = (string)found.GetValue("Info");

				return found;
			}
		}


		public class Customer {
			public ObjectId Id { get; set; }
			public string Name { get; set; }
			public string Surname { get; set; }

			public Customer Find() {
				Console.WriteLine("Please enter customer Name : ");
				Name = Console.ReadLine();

				Console.WriteLine("Please enter customer Surname : ");
				Surname = Console.ReadLine();

				var found = customer.FindSync<Customer>(
					new BsonDocument("$and",
					new BsonArray {
						new BsonDocument( "Name", new BsonDocument("$regex", Name) ),
						new BsonDocument( "Surname", new BsonDocument("$regex", Surname) )
					})).FirstOrDefault();

				if(found == null) {
					Console.WriteLine("Nothing found. Try again : ");
					return this.Find();
				}
				//Id = (ObjectId)found.GetValue("_id");
				//Name = (string)found.GetValue("Name");
				//Surname = (string)found.GetValue("Surname");

				return found;
			}
		}


		public class Staff {
			public ObjectId Id { get; set; }
			public string Name { get; set; }
			public string From { get; set; }
			public string To { get; set; }
			public string Info { get; set; }

			public Staff Find() {
				Console.WriteLine("Please enter staff Name : ");
				Name = Console.ReadLine();

				var found = staff.FindSync<Staff>(new BsonDocument("Name", new BsonDocument("$regex", Name))).FirstOrDefault();

				if(found == null) {
					Console.WriteLine("Nothing found. Try again : ");
					return this.Find();
				}
				//Id = (ObjectId)found.GetValue("_id");
				//Name = (string)found.GetValue("Name");
				//From = (string)found.GetValue("From");
				//To = (string)found.GetValue("To");
				//Info = (string)found.GetValue("Info");

				return found;
			}
		}

	}


}
