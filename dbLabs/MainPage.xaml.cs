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
//using Syncfusion.Data.Extensions;
using AppKit;
using dbLabs.Classes;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace dbLabs {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage {

		//private string connString = "server=127.0.0.1; user=root; password=Password; database=autoschool";

		private ShopContext context;

		public MainPage() {

			using(ShopContext db = new ShopContext()) {
				// Creates the database if not exists
				if(db.Database.EnsureCreated() == true) {

					Contract contract1 = new Contract { Info = "From now to then 553" };
					Contract contract2 = new Contract { Id = 20, Info = "From now to then 323" };
					Contract contract3 = new Contract { Info = "From now to then 400" };
					db.Contracts.AddRange(new List<Contract>() { contract1, contract2, contract3 });
					db.SaveChanges();

					Staff staff1 = new Staff { Name="Dan", Contract = contract1 };
					Staff staff2 = new Staff { Name = "Ila", Contract = contract2 };
					Staff staff3 = new Staff { Name = "Lad", Contract = contract3 };
					db.Staffs.AddRange(new List<Staff>() { staff1, staff2, staff3 });
					db.SaveChanges();


					Provider provider1 = new Provider { Name = "BGD", Info = "Good stuff" };
					Provider provider2 = new Provider { Name = "LFQ", Info = "Best stuff" };
					db.Providers.AddRange(new List<Provider>() { provider1, provider2 });
					db.SaveChanges();

					Product product1 = new Product { Name = "fftp1" };
					Product product2 = new Product { Name = "fftp2" };
					Product product3 = new Product { Name = "fftp3" };
					Product product4 = new Product { Name = "fftp4" };
					db.Products.AddRange(new List<Product>() { product1, product2, product3, product4 });
					db.SaveChanges();


					ShopItem sItem1 = new ShopItem { Amount = 10421, Price = 1, Product = product1, Provider = provider1 };
					ShopItem sItem2 = new ShopItem { Amount = 20425, Price = 2, Product = product1, Provider = provider2 };
					ShopItem sItem3 = new ShopItem { Amount = 5424, Price = 2, Product = product2, Provider = provider1 };
					ShopItem sItem4 = new ShopItem { Amount = 25423, Price = 4, Product = product2, Provider = provider2 };
					ShopItem sItem5 = new ShopItem { Amount = 2424, Price = 3, Product = product3, Provider = provider1 };
					ShopItem sItem6 = new ShopItem { Amount = 5423, Price = 6, Product = product3, Provider = provider2 };
					ShopItem sItem7 = new ShopItem { Amount = 424, Price = 4, Product = product4, Provider = provider1 };
					ShopItem sItem8 = new ShopItem { Amount = 5843, Price = 8, Product = product4, Provider = provider2 };

					db.ShopItems.AddRange(new List<ShopItem>() { sItem1, sItem2, sItem3, sItem4, sItem5, sItem6, sItem7, sItem8 });
					db.SaveChanges();

					Customer customer1 = new Customer { Name = "LRt"};
					Customer customer2 = new Customer { Name = "BigBrained" };
					Customer customer3 = new Customer { Name = "Autoencoder" };
					Customer customer4 = new Customer { Name = "Pythonist" };
					db.Customers.AddRange(new List<Customer>() { customer1, customer2, customer3, customer4 });
					db.SaveChanges();

					db.Purchases.AddRange(new List<Purchase>() {
						new Purchase { Amount = 10, Customer = customer1, ShopItem = sItem1, Staff = staff1 },
						new Purchase { Amount = 9, Customer = customer2, ShopItem = sItem7, Staff = staff3 },
						new Purchase { Amount = 4, Customer = customer4, ShopItem = sItem6, Staff = staff3 },
						new Purchase { Amount = 6, Customer = customer2, ShopItem = sItem3, Staff = staff2 },
						new Purchase { Amount = 3, Customer = customer3, ShopItem = sItem2, Staff = staff1 },
						new Purchase { Amount = 2, Customer = customer1, ShopItem = sItem1, Staff = staff2 },
						new Purchase { Amount = 7, Customer = customer4, ShopItem = sItem6, Staff = staff2 },
						new Purchase { Amount = 3, Customer = customer2, ShopItem = sItem3, Staff = staff3 },
						new Purchase { Amount = 8, Customer = customer3, ShopItem = sItem7, Staff = staff1 },
						new Purchase { Amount = 5, Customer = customer1, ShopItem = sItem8, Staff = staff1 }
					});
					db.SaveChanges();

					/*
					//
					Candidate c1 = new Candidate { Name = "Alex", Surname = "Lukashenko", Rating = 35 };
					Candidate c2 = new Candidate { Name = "Zianon", Surname = "Pozniak", Rating = 65 };
					Candidate c3 = new Candidate { Name = "Maria", Surname = "Vasilevish", Rating = 0 };
					//db.Candidates.AddRange(new List<Candidate>() { c1, c2, c3 });
					db.Candidates.Add(c1);
					db.Candidates.Add(c2);
					db.Candidates.Add(c3);
					db.SaveChanges();

					Confident conf1 = new Confident { FullName = "Alex Matskevich", Age = 40, PoliticalPreferences = "Neutral", Candidate = c1 };
					Confident conf2 = new Confident { FullName = "Karyna Kluchnik", Age = 18, PoliticalPreferences = "Monarch", Candidate = c2 };
					Confident conf3 = new Confident { FullName = "Tanya Verbovich", Age = 25, PoliticalPreferences = "Liberal", Candidate = c2 };
					//db.Confidents.AddRange(new List<Confident>() { conf1, conf2, conf3 });
					db.Confidents.Add(conf1);
					db.Confidents.Add(conf2);
					db.Confidents.Add(conf3);

					Promise prom1 = new Promise { Text = "Stop War" };
					Promise prom2 = new Promise { Text = "Increase revenue" };

					// many-to-many
					prom1.CandidatePromise = new List<CandidatePromise> {
					new CandidatePromise {Candidate = c1, Promise = prom1},
					new CandidatePromise {Candidate = c2, Promise = prom1},
				};
					prom2.CandidatePromise = new List<CandidatePromise> {
					new CandidatePromise {Candidate = c1, Promise = prom2}
				};

					db.Promises.AddRange(new List<Promise> { prom1, prom2 });
					//db.Promises.Add(prom1);
					//db.Promises.Add(prom2);

					db.SaveChanges();
					CandidateProfile prof1 = new CandidateProfile { Id = c1.Id, Age = 50, Description = "Current president", Candidate = c1 };
					CandidateProfile prof2 = new CandidateProfile { Id = c2.Id, Age = 40, Description = "Legend", Candidate =c2 };
					//db.CandidateProfiles.AddRange(new List<CandidateProfile> { prof1, prof2 });
					db.CandidateProfiles.Add(prof1);
					db.CandidateProfiles.Add(prof2);

					db.SaveChanges();
					*/

					/*
					prom1.Candidates.Add(c1);
					prom2.Candidates.Add(c1);
					prom2.Candidates.Add(c2);
					*/
				}
			}

			InitializeComponent();


		}

		public void PageLoaded(object sender, EventArgs e) {
			context = new ShopContext();
			context.Staffs.Load();
			ShowGrid.ItemsSource = context.Staffs.Local.ToBindingList();
			//

			/////////////////////
			// Greedy (not virtual props)
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
			// Lazy (set virtual props) delayed
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
			// Explicit (not virtual props) delayed
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

		private void RemoveItem(object sender, EventArgs e) {
			try {
				if(ShowGrid.SelectedItems != null) {
					for(int i = 0; i < ShowGrid.SelectedItems.Count; i++) {
						Staff curCand = ShowGrid.SelectedItems[i] as Staff; if(curCand != null) {
							context.Staffs.Remove(curCand);
						}
					}
				}
				context.SaveChanges();
			} catch(Exception ex) {
				System.Console.WriteLine(ex.Message);
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
	}
}
