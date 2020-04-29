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

		private ElectionContext curCont;

		public MainPage() {

			using(ElectionContext db = new ElectionContext()) {
				// Creates the database if not exists
				if(db.Database.EnsureCreated() == true) {
					
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
			curCont = new ElectionContext();
			curCont.Candidates.Load();
			ShowGrid.ItemsSource = curCont.Candidates.Local.ToBindingList();
			//

			/////////////////////
			// Greedy (not virtual props)
			/////////////////////
			/*
			List<Candidate> curCand = curCont.Candidates.Include(p => p.Confidents).ToList<Candidate>();
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
			var curCand1 = curCont.Candidates.ToList();
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
			var curCand = curCont.Candidates.FirstOrDefault();
			curCont.Entry(curCand).Collection("Confidents").Load();
			string result = "";
			result += curCand.Name + " " + curCand.Surname + " \n" + "Confidents: ";
			foreach(var conf in curCand.Confidents) {
				result += conf.FullName + " ";
			}
			*/

		}
		public void PageClosed(object sender, EventArgs e) {
			curCont.Dispose();
		}

		private void Sync(object sender, EventArgs e) {
			curCont.SaveChanges();
		}
		private void RemoveItem(object sender, EventArgs e) {
			try {
				if(ShowGrid.SelectedItems != null) {
					for(int i = 0; i < ShowGrid.SelectedItems.Count; i++) {
						Candidate curCand = ShowGrid.SelectedItems[i] as Candidate; if(curCand != null) {
							curCont.Candidates.Remove(curCand);
						}
					}
				}
				curCont.SaveChanges();
			} catch(Exception ex) {
				System.Console.WriteLine(ex.Message);
			}
		}

		private void Rating_Click(object sender, EventArgs e) {
			Candidate firstCandidate = curCont.Candidates.FirstOrDefault();
			System.Random rnd = new System.Random();
			int myInt = rnd.Next(1, 10000);
			float myFloat = myInt / 100;
			firstCandidate.RatingChange(myFloat);

			Candidate secondCandidate = curCont.Candidates.OrderBy(x => x.Id).Skip(1).FirstOrDefault();
			secondCandidate.RatingChange(100 - myFloat); curCont.SaveChanges();
			ShowGrid.Refresh();
		}

		private void Edit_Surname(object sender, EventArgs e) {
			if(ShowGrid.SelectedItem != null) {
				Candidate curCand = ShowGrid.SelectedItem as Candidate;
				curCand.Surname = editSurname.Text;
				curCont.SaveChanges();
				ShowGrid.Refresh();
			}
		}

		private void query_Click(object sender, EventArgs e) {
			using(ElectionContext db = new ElectionContext()) {
				var result = db.Candidates.Where(x => x.Rating > 10).ToList();
				
				var result2 = db.Candidates.Join(db.Confidents,
					  p => p.Id,
					  c => c.CandidateId,
					  (p, c) => new {
						  Name = c.FullName,
						  CandidateSurname = p.Surname
					  }).ToList();

				var result3 = db.Confidents.AsEnumerable()
					.GroupBy(c => c.CandidateId)
					.Select(g => new { ID = g.FirstOrDefault().Id, AvgAge = g.Average(c => c.Age) })
					.Join(db.Candidates.AsEnumerable(),
					a => a.ID,
					b => b.Id,
					(a, b) => new {
						Name = b.Name,
						Surname = b.Surname,
						Age = a.AvgAge
					}
					).ToList();
				resultGrid.ItemsSource = result2;
			}
		}
	}
}
